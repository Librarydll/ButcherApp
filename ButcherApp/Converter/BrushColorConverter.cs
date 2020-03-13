using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ButcherApp.Converter
{
	public class BrushColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				{
					return (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF46FB46"));
				}
			}
			
			return (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFF4000"));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}
