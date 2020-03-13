using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace ButcherApp.Models
{
	public class RecSetting : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public bool Key { get; set; }
	
		public bool Flag { get; set; }
		
		public bool Print { get; set; }

		public bool HistName { get; set; }

		public bool Date { get; set; }

		public bool Time { get; set; }

		public bool HeadName { get; set; }
	
		public bool HeadSide { get; set; }

		public bool HeadProgres { get; set; }

		public bool Tare { get; set; }

		public bool Net { get; set; }

		public bool Gross { get; set; }

		public bool Flow { get; set; }

		public bool ProductCode { get; set; }

		public bool ProductName { get; set; }

		public bool ProductProgres { get; set; }

		public bool ProductTot { get; set; }

		public bool Client { get; set; }

		public bool Expiry { get; set; }

		public bool Note0 { get; set; }

		public bool Note1 { get; set; }

		public bool Note2 { get; set; }

		public bool Note3 { get; set; }

		public bool Note4 { get; set; }

		public bool Note5 { get; set; }

		public bool Note6 { get; set; }

		public bool FillerProgres { get; set; }

		public bool YearCode { get; set; }

		public bool DayCode { get; set; }

		public bool PlantCode { get; set; }

		public bool Plant { get; set; }

		public bool ShiftCode { get; set; }

		public bool CapSter { get; set; }

		public bool Container { get; set; }

		public bool DecimalSymbol { get; set; }

		public bool BarCode { get; set; }


		public static explicit operator RecSetting(string[] array)
		{
			RecSetting setting = new RecSetting();
			var type = typeof(RecSetting);
			foreach (var prop in type.GetProperties())
			{
				foreach (var item in array)
				{
					if (item.Equals(prop.Name))
						type.GetProperty(item).SetValue(setting, true);
				}
			}

			return setting;
		}
	}
}
