using ButcherApp.Converter;
using ButcherApp.Models;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ButcherApp.ViewModels
{
	public class ShellViewModel :Screen
	{	
		private BindableCollection<Rec> _dataEntry;

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



		public async Task OpenXmlFile()
		{
			OpenFileDialog openFile = new OpenFileDialog();
			XMLConvert<List<Rec>> convert = new XMLConvert<List<Rec>>();
			//if (openFile.ShowDialog() == true)
			//{
			//	//await convert.ChangeDocumnet(openFile.FileName);
			//	//var temp = convert.DeSerialize(openFile.FileName);
			//	//_dataEntry = new BindableCollection<Rec>(temp);
			//}
			await convert.ChangeDocumnet(@"C:\Users\Komil\Desktop\new.dat");
			var temp = convert.DeSerialize(@"C:\Users\Komil\Desktop\new.dat");
			DataEntry = new BindableCollection<Rec>(temp);
			var netSum = DataEntry.Sum(x => x.Net);
			var count = DataEntry.Count;

			Overall = string.Format($"Sum of Net  = {netSum}  Count = {count} ");
		}

	}
}
