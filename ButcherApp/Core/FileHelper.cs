using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ButcherApp.Core
{
    class FileHelper
    {
		public static Tuple<DirectoryInfo,IEnumerable<FileInfo>> GetLocalFiles(string path)
		{
			DirectoryInfo directory = new DirectoryInfo(path);
			var files = directory.GetFiles()
				.Where(x => x.FullName.Contains("PX"))
				.Where(x => x.Extension.ToLower() == ".dat");


			return Tuple.Create(directory, files);

		}
	}
}
