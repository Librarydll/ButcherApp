﻿using ButcherApp.ViewModels;
using Caliburn.Micro;
using System.Windows;

namespace ButcherApp
{
	public class Bootstrapper :BootstrapperBase
	{
		public Bootstrapper()
		{
			Initialize();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<ShellViewModel>();
		}
	}
}
