using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace ButcherApp.Converter
{
	public class XmlSetting
	{
		readonly string _fileName = "settings.xml";
	    string _filePath = "";
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
			try
			{
				document.Load(_fileName);
			}
			catch (XmlException ex)
			{

				MessageBox.Show(ex.Message);
			}
			
			XmlNode pathNode = document.DocumentElement.SelectSingleNode("FilePath");
			FilePath = pathNode.InnerText;
		}

	}
}
