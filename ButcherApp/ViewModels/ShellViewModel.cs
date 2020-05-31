using ButcherApp.Converter;
using ButcherApp.Core;
using ButcherApp.Models;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ButcherApp.ViewModels
{
	public class ShellViewModel : Screen
	{
		private BindableCollection<Rec> _dataEntry;
		private BindableCollection<Rec> _tempDataEntry;
		private List<DateTime> _sortingDate = new List<DateTime>();
		private XmlSetting setting = null;
		private string _overall;
		private string _folderPathName;
		private DateTime _startDateTime = DateTime.Now;
		private DateTime _endDateTime = DateTime.Now;
		private readonly string _saveExcelDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Excel Documents";
		private readonly IWindowManager windowManager;
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

		private int _progress;

		public int ProgressValue
		{
			get { return _progress; }
			set { _progress = value; NotifyOfPropertyChange(() => ProgressValue); }
		}

		private int _maxValue = 100;

		public int MaxValue
		{
			get { return _maxValue; }
			set { _maxValue = value; NotifyOfPropertyChange(() => MaxValue); }
		}


		public List<string> FilterName
		{
			get { return new List<string> { "HeadProgres", "ProductProgres", "Tare", "Net", "Gross", "Flow", "ProductTot", "HeadName", "Note" }; }
		}

		public string SelectedFilter
		{
			get { return _selectedFilter; }
			set { _selectedFilter = value; NotifyOfPropertyChange(() => SelectedFilter); }
		}

		private string _searchingString;
		private TimeWatcher _selectedTime = new TimeWatcher();
		private bool _isTimeEnable;
		private CalendarBlackoutDatesCollection _blackoutDates;

		public string SearchingString
		{
			get { return _searchingString; }
			set
			{
				if (string.IsNullOrWhiteSpace(value) && _tempDataEntry != null)
				{
					DataEntry = _tempDataEntry;
					SetOverall();
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

		public TimeWatcher SelectedTime { get => _selectedTime; set { _selectedTime = value; NotifyOfPropertyChange(); } }
		public bool IsTimeEnable { get => _isTimeEnable; set { _isTimeEnable = value; NotifyOfPropertyChange(); } }

		public CalendarBlackoutDatesCollection BlackoutDates { get => _blackoutDates; set { _blackoutDates = value; NotifyOfPropertyChange(); } }

		public ShellViewModel(IWindowManager windowManager)
		{
			this.windowManager = windowManager;
			BlackoutDates = new CalendarBlackoutDatesCollection(new Calendar());
			setting = new XmlSetting(string.Empty);
			var path = setting.FilePath;
			FolderPathName = false == string.IsNullOrWhiteSpace(path) ? path : string.Empty;
			//Task.Run(async () => await LoadBlackOutDates());
			LoadBlackOutDates();
		}

		public async Task OpenXmlFile()
		{
			if (string.IsNullOrWhiteSpace(setting.FilePath))
			{
				SetFolder();
				return;
			}
			if (DateTime.Compare(StartDateTime, EndDateTime) == 1)
			{
				MessageBox.Show("Invalid Filter");
				return;
			}
			XMLConvert<List<Rec>> convert = new XMLConvert<List<Rec>>();
			var fileheper =FileHelper.GetLocalFiles(setting.FilePath);

			var files = fileheper.Item2;
			FolderPathName = fileheper.Item1.FullName;

			ProgressModel prModel = new ProgressModel();
			IProgress<ProgressModel> progress = new Progress<ProgressModel>((value) =>
			{
				ProgressValue += value.Percentage;
			});
			var prog = progress as Progress<ProgressModel>;
			prog.ProgressChanged += Progress_ProgressChanged;

			List<Rec> temp = new List<Rec>();
			List<Rec> temp2 = new List<Rec>();
			int i = 0;
			MaxValue = files.Count();

			await Task.Run(async () =>
		   {
			   foreach (var file in files)
			   {

				   var date = file.FullName.FormatDate();
				   i++;
				   if (date >= StartDateTime.Date && date <= EndDateTime.Date)
				   {
					   prModel.Percentage = i;
					   progress.Report(prModel);
					   await convert.ChangeDocumnet(file.FullName);
					   temp = convert.DeSerialize(file.FullName);
					   if (temp == null) continue;
					   if (IsTimeEnable)
					   {
						   temp = temp.FilterByTime(SelectedTime.FromTime, SelectedTime.ToTime)
											.ToList();
					   }

					   temp2.AddRange(temp);
					   _sortingDate.Add(date);
				   }
			   }
		   });

			DataEntry = new BindableCollection<Rec>(temp2);
			_tempDataEntry = DataEntry;
			SetOverall();
			prModel.Percentage = 0;
			progress.Report(prModel);

		}

		private void Progress_ProgressChanged(object sender, ProgressModel e)
		{
			ProgressValue = e.Percentage;
		}

		public void SetFolder()
		{
			OpenFileDialog openFile = new OpenFileDialog();

			if (openFile.ShowDialog() == true)
			{
				setting = new XmlSetting(Path.GetDirectoryName(openFile.FileName));
				FolderPathName = Path.GetDirectoryName(openFile.FileName);

				LoadBlackOutDates();
			}
		}
		public async Task ExportToExcel()
		{
			if (DataEntry?.Count == 0 || DataEntry == null)
			{
				MessageBox.Show("Data is empty");
				return;
			}
			IProgress<ProgressModel> progress = new Progress<ProgressModel>((value) =>
			{
				ProgressValue += value.Percentage;
			});
			var prog = progress as Progress<ProgressModel>;
			prog.ProgressChanged += Progress_ProgressChanged;
			MaxValue = _dataEntry.Count;
			ExcelDocument excel = new ExcelDocument();
			_saveExcelDocumentsPath.CreateFolder();
			await excel.Export(_saveExcelDocumentsPath + Path.Combine($"\\{_sortingDate.FirstOrDefault().ToShortDateString()}_{_sortingDate.LastOrDefault().ToShortDateString()}.xlsx"),
				_dataEntry.ToList(), progress);
		}
		public void SearchData()
		{
			int _value = 0;
			if (DataEntry == null || DataEntry.Count == 0) return;
			switch (SelectedFilter)
			{
				case "HeadProgres":
					if (Int32.TryParse(SearchingString, out _value))
						DataEntry = _tempDataEntry.Where(x => x.HeadProgres == _value).ToBindable();
					break;
				case "Tare":
					if (Int32.TryParse(SearchingString, out _value))
						DataEntry = _tempDataEntry.Where(x => x.Tare == _value).ToBindable();
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
						DataEntry = _tempDataEntry.Where(x => Convert.ToString(x.ProductProgres).Contains(val.ToString())).ToBindable();
					break;
				case "HeadName":
					DataEntry = _tempDataEntry.Where(x => x.HeadName.Contains(SearchingString.ToUpper())).ToBindable();
					break;
				case "Note":
					DataEntry = _tempDataEntry.Where(x => x.Note1.Contains(SearchingString.ToLower())).ToBindable();
					break;
				case "ProductProgres":

					var splited = SearchingString.Split().Where(x => !string.IsNullOrWhiteSpace(x));

					if (splited.Count() >= 2)
					{
						if (Int32.TryParse(splited.ElementAt(0), out _value) && Int32.TryParse(splited.ElementAt(1), out int _value2))
						{
							DataEntry = _tempDataEntry.Where(x => x.ProductProgres >= _value && x.ProductProgres <= _value2).ToBindable();
						}

					}


					break;

				default:
					break;
			}
			SetOverall();
		}

		public  void LoadBlackOutDates()
		{
			if (!Directory.Exists(setting.FilePath)) return;
			var fileheper = FileHelper.GetLocalFiles(setting.FilePath);
			var files = fileheper.Item2;

			foreach (var file in files)
			{
				var date = file.FullName.FormatDate();
				BlackoutDates.Add(new CalendarDateRange(date));
			}
		}



		public void PreviewKeyDownEventHandler(KeyEventArgs e)
		{
			if (e.Key == Key.F1 && Keyboard.IsKeyDown(Key.LeftCtrl))
			{
				Task.Run(async () => await ExportToExcel());
			}
			else if (e.Key == Key.F2 && Keyboard.IsKeyDown(Key.LeftCtrl))
			{
				Navigate();
			}
			else if (e.Key == Key.F3 && Keyboard.IsKeyDown(Key.LeftCtrl))
			{
				ExcelSetting();
			}
		}

		public void SetOverall()
		{
			var netSum = DataEntry.Sum(x => x.Net);
			var count = DataEntry.Count;

			Overall = string.Format($"Sum of Net  = {netSum}  Count = {count} ");
		}
		public void Navigate()
		{

			if (Directory.Exists(_saveExcelDocumentsPath))
			{
				Process.Start(_saveExcelDocumentsPath);
			}
		}

		public void ExcelSetting()
		{
			windowManager.ShowDialog(new SettingViewModel());
		}
	}
}
