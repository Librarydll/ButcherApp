using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButcherApp.Models
{
	public class TimeWatcher : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public TimeSpan FromTime { get; set; } = DateTime.Now.TimeOfDay;
		public TimeSpan ToTime { get; set; } = DateTime.Now.TimeOfDay;

	}
}
