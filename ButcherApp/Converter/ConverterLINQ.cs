using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ButcherApp.Converter
{
	public static class ConverterLINQ
	{

		public static BindableCollection<DateTime> FormatChange(this List<string> collection)
		{
			BindableCollection<DateTime> newCollection = new BindableCollection<DateTime>();
			foreach (var data in collection)
			{
				newCollection.Add(Convert.ToDateTime(data));			
			}
			return newCollection;
		}

		public static DateTime FormatDate(this string str)
		{
			str = Path.GetFileNameWithoutExtension(str);
			var newStr = str.Remove(0, 2);
			DateTime date = DateTime.Now;
			try
			{
				date = DateTime.ParseExact(newStr, "yyMMdd", CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				MessageBox.Show($"Name this file {str} is invalid","File naming error",MessageBoxButton.OK,MessageBoxImage.Error);
			}
			return date;
		}

		public static void CreateFolder(this string str)
		{
			if (Directory.Exists(str))
				return;
			Directory.CreateDirectory(str);
		}

	}
}
