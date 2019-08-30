using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ButcherApp.Converter
{
 	public class XMLConvert <T> where T:class
	{
		private XmlSerializer serializer = new XmlSerializer(typeof(T));

		public T DeSerialize(string path)
		{
			T _obj = null;
			using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				_obj = (T)serializer.Deserialize(stream);
			}
			return _obj;
		}

		public void SerializeXml(T obj, string path)
		{
			using (Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
			{
				serializer.Serialize(stream, obj);
			}
		}
		public async Task ChangeDocumnet(string path)
		{
			string file = null;
			using (var reader = new StreamReader(path))
			{
				file = reader.ReadToEnd();

				if (file.Contains("<ArrayOfRec xmlns:xsi="))
					return;
			}
				using (var writer = new StreamWriter(path))
				{
					await writer.WriteLineAsync(@"<?xml version=""1.0""?>");
					await writer.WriteLineAsync(@"<ArrayOfRec xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">");
					var bytes = file.ToCharArray();
					await writer.WriteAsync(bytes, 0, bytes.Length);
					await writer.WriteLineAsync("</ArrayOfRec>" + Environment.NewLine);
				}
		}		
	}
}
