using ButcherApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ButcherApp.Core
{
	public class EcxelSettingHelper
	{
		public EcxelSettingHelper()
		{
			CreateFile();
		}
		private static readonly string fileName = "excelsettings.txt";
		public string[] GetSettingNames()
		{
			IList<string> result = new List<string>();

			try
			{
				using (var reader = new StreamReader(fileName))
				{
					while (!reader.EndOfStream)
					{
						result.Add(reader.ReadLine());
					}
				}
				return result.ToArray();

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			return null;
		}
		public void SetSettingNames(RecSetting recSetting)
		{
			IList<string> result = new List<string>();
			try
			{
				var type = typeof(RecSetting);
				using (var writer = new StreamWriter(fileName))
				{
					foreach (var prop in type.GetProperties())
					{
						if ((bool)prop.GetValue(recSetting))
						{
							writer.WriteLine(prop.Name);
						}
					}
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}	
		public static void CreateFile()
		{
			if (!File.Exists(fileName))
			{
				File.Create(fileName);
			}
		}
	}
}
