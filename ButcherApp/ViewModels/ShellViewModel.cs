using ButcherApp.Converter;
using ButcherApp.Models;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ButcherApp.ViewModels
{
	public class ShellViewModel :Screen
	{	
		private BindableCollection<Rec> _dataEntry;
		private BindableCollection<Rec> _tempDataEntry;
		private List<DateTime> _sortingDate = new List<DateTime> ();
		private XmlSetting setting = null;
		private string _overall;
		private string _folderPathName;
		private DateTime _startDateTime;
		private DateTime _endDateTime;
		private readonly string _saveExcelDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Excel Documents";
		private string _selectedFilter;
		public string FolderPathName
		{
			get { return _folderPathName; }
			set
			{
				_folderPathName = value;
				NotifyOfPropertyChange(() => FolderPathName);
			}
		}

		public List<string> FilterName
		{
			get { return new List<string> { "HeadProgres","Tare","Net","Gross","Flow", "ProductTot", "HeadName","Note" }; }
		}

		public string SelectedFilter
		{
			get { return _selectedFilter; }
			set { _selectedFilter = value; NotifyOfPropertyChange(() => SelectedFilter); }
		}

		private string _searchingString;

		public string SearchingString
		{
			get { return _searchingString; }
			set {
				if (string.IsNullOrWhiteSpace(value) && _tempDataEntry != null)
				{
					DataEntry = _tempDataEntry;
					var netSum = DataEntry.Sum(x => x.Net);
					var count = DataEntry.Count;

					Overall = string.Format($"Sum of Net  = {netSum}  Count = {count} ");
				}
				_searchingString = value; NotifyOfPropertyChange(() => SearchingString);
				}
		}


		public BindableCollection<Rec> DataEntry
		{
			get { return _dataEntry; }
			set
			{
				_dataEntry = value;
				NotifyOfPropertyChange(() => DataEntry);
			}
		}

		public string Overall
		{
			get { return _overall; }
			set { _overall = value; NotifyOfPropertyChange(() => Overall); }
		}

		public DateTime StartDateTime
		{
			get { return _startDateTime; }
			set
			{
				_startDateTime = value;
				NotifyOfPropertyChange(() => StartDateTime);
			}
		}

		public DateTime EndDateTime
		{
			get { return _endDateTime; }
			set
			{
				_endDateTime = value;
				NotifyOfPropertyChange(() => EndDateTime);
			}
		}
		public ShellViewModel()
		{
			StartDateTime = DateTime.Now;
			EndDateTime = DateTime.Now;
			setting = new XmlSetting(string.Empty);
			if (string.IsNullOrWhiteSpace(setting.FilePath))
				return;
			var path = setting.FilePath;
			FolderPathName = false == string.IsNullOrWhiteSpace(path) ? path : string.Empty;
		}

		public async Task OpenXmlFile()
		{
			if (string.IsNullOrWhiteSpace(setting.FilePath))
			{
				SetFolder();
				return;
			}
			if(DateTime.Compare(StartDateTime,EndDateTime)==1)
			{
				MessageBox.Show("Invalid Filter");
				return;
			}
			XMLConvert<List<Rec>> convert = new XMLConvert<List<Rec>>();
			DirectoryInfo directory = new DirectoryInfo(setting.FilePath);
			FileInfo[] files = directory.GetFiles();
			string[] filteredNames=new string[files.Length];
			FolderPathName = directory.FullName;
			List<Rec> temp = new List<Rec> ();
			List<Rec> temp2 = new List<Rec>();
			foreach (var file in files)
			{
				if (file.Extension.ToLower() != ".dat")
					continue;
				var date = file.FullName.FormatDate();
				if (date >= StartDateTime.Date && date <= EndDateTime.Date)
				{
					await convert.ChangeDocumnet(file.FullName);
					temp = convert.DeSerialize(file.FullName);
					temp2.AddRange(temp);
					_sortingDate.Add(date);
				}
			}
			DataEntry = new BindableCollection<Rec>(temp2);
			_tempDataEntry = DataEntry;
			var netSum = DataEntry.Sum(x => x.Net);
			var count = DataEntry.Count;

			Overall = string.Format($"Sum of Net  = {netSum}  Count = {count} ");
		}

		public void SetFolder()
		{
			OpenFileDialog openFile = new OpenFileDialog();

			if (openFile.ShowDialog() == true)
			{
				setting = new XmlSetting(Path.GetDirectoryName(openFile.FileName));
				FolderPathName = Path.GetDirectoryName(openFile.FileName);
			}
		}
		public void ExportToExcel()
		{
			if (DataEntry?.Count == 0||DataEntry==null)
			{
				MessageBox.Show("Data is empty");
				return;
			}
			ExcelDocument excel = new ExcelDocument();
			_saveExcelDocumentsPath.CreateFolder();
			excel.Export(_saveExcelDocumentsPath + Path.Combine ($"\\{_sortingDate.FirstOrDefault().ToShortDateString()}_{_sortingDate.LastOrDefault().ToShortDateString()}.xlsx"),
				_dataEntry.ToList());
		}

		public void SearchData()
		{
			int _value = 0;

			switch (SelectedFilter)
			{
				case  "HeadProgres":
					if (Int32.TryParse(SearchingString, out _value))
						DataEntry = _tempDataEntry.Where(x => x.HeadProgres == _value).ToBindable();
					break;
				case "Tare":
					if(Int32.TryParse(SearchingString,out _value))
						DataEntry = _tempDataEntry.Where(x => x.Tare== _value).ToBindable();
					break;
				case "Net":
					if (Int32.TryParse(SearchingString, out _value))
						DataEntry = _tempDataEntry.Where(x => x.Net == _value).ToBindable();
					break;
				case "Gross":
					if (Int32.TryParse(SearchingString, out _value))
						DataEntry = _tempDataEntry.Where(x => x.Gross == _value).ToBindable();
					break;
				case "Flow":
					if (Int32.TryParse(SearchingString, out _value))
						DataEntry = _tempDataEntry.Where(x => x.Flow == _value).ToBindable();
					break;
				case "ProductTot":
					double val = 0.0;
					if (Double.TryParse(SearchingString, out val))
						DataEntry = _tempDataEntry.Where(x => Convert.ToString(x.ProductProgres).Contains(val.ToString()) ).ToBindable();
					break;
				case "HeadName":
						DataEntry = _tempDataEntry.Where(x => x.HeadName.Contains(SearchingString.ToUpper())).ToBindable();
					break;
				case "Note":
					DataEntry = _tempDataEntry.Where(x => x.Note1.Contains(SearchingString.ToLower())).ToBindable();
					break;

				default:
					break;
			}
			var netSum = DataEntry.Sum(x => x.Net);
			var count = DataEntry.Count;

			Overall = string.Format($"Sum of Net  = {netSum}  Count = {count} ");
		}

	}
}
