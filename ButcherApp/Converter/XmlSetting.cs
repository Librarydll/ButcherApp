using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace ButcherApp.Converter
{
	public class XmlSetting
	{
		readonly string _fileName = "settings.xml";
	    string _filePath = "";
		bool folderIsCreated = false;
		public string FilePath
		{
			get
			{
				return _filePath;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					return;
				_filePath = value;
				SaveSettings();
			}
		}

		public XmlSetting(string filePath)
		{
			if (!File.Exists(_fileName))
			{
				File.Create(_fileName);
			}
			FilePath = filePath;
			folderIsCreated = true;
			GetFilePath();
		}
		private void SaveSettings()
		{
				XDocument xDocument = new XDocument(
					new XElement("root",
					new XElement("FilePath", _filePath)));
				
			xDocument.Save(_fileName);		
		}

		private void GetFilePath()
		{
			XmlDocument document = new XmlDocument();
			document.Load(_fileName);
			XmlNode pathNode = document.DocumentElement.SelectSingleNode("FilePath");
			FilePath = pathNode.InnerText;
		}

	}
}
