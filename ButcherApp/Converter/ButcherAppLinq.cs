using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace ButcherApp.Converter
{
	public static class ButcherAppLinq
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

		public static  DateTime FormatDate(this string str)
		{
			str = Path.GetFileNameWithoutExtension(str);
			var newStr = string.Concat(str.ToCharArray().Where(x => char.IsDigit(x)));
			DateTime date = DateTime.Now;
			date = DateTime.ParseExact(newStr, "yyMMdd", CultureInfo.InvariantCulture);
			return date;
		}

		public static void CreateFolder(this string str)
		{
			if (Directory.Exists(str))
				return;
			Directory.CreateDirectory(str);
		}
		
		public static BindableCollection<T> ToBindable<T>(this IEnumerable<T> collection)
		{
			return new BindableCollection<T>(collection);
		}

	}
}
