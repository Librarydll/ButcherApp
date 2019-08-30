using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
