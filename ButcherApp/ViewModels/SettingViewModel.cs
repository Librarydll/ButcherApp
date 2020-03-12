using ButcherApp.Core;
using ButcherApp.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButcherApp.ViewModels
{
	public class SettingViewModel:Screen
	{
		EcxelSettingHelper _fileHelper = new EcxelSettingHelper();
		private RecSetting _excelSetting;

		public RecSetting ExcelSetting
		{
			get { return _excelSetting; }
			set { _excelSetting = value; NotifyOfPropertyChange(); }
		}
		public SettingViewModel()
		{
			ExcelSetting = new RecSetting();
			ExcelSetting =(RecSetting)_fileHelper.GetSettingNames();
		}


		public void Confirm()
		{
			_fileHelper.SetSettingNames(ExcelSetting);
			this.TryClose();
		}
	}
}
