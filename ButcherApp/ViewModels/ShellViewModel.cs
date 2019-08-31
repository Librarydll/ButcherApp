using ButcherApp.Converter;
using ButcherApp.Models;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
		private XmlSetting setting = null;

		private string _folderPathName;

		public string FolderPathName
		{
			get { return _folderPathName; }
			set
			{
				_folderPathName = value;
				NotifyOfPropertyChange(() => FolderPathName);
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
		private string _overall;

		public string Overall
		{
			get { return _overall; }
			set { _overall = value; NotifyOfPropertyChange(() => Overall); }
		}

		public ShellViewModel()
		{
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

			XMLConvert<List<Rec>> convert = new XMLConvert<List<Rec>>();
			DirectoryInfo directory = null;

			directory = new DirectoryInfo(setting.FilePath);
			FileInfo[] files = directory.GetFiles();
			FolderPathName = directory.FullName;
			//await convert.ChangeDocumnet(openFile.FileName);
			//var temp = convert.DeSerialize(openFile.FileName);
			//DataEntry = new BindableCollection<Rec>(temp);
			//await convert.ChangeDocumnet(@"C:\Users\Komil\Desktop\new.dat");
			//var temp = convert.DeSerialize(@"C:\Users\Komil\Desktop\new.dat");
			//DataEntry = new BindableCollection<Rec>(temp);
			//	var netSum = DataEntry.Sum(x => x.Net);
			//var count = DataEntry.Count;

			//Overall = string.Format($"Sum of Net  = {netSum}  Count = {count} ");
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

	}
}
