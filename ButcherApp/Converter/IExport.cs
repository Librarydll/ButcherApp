using ButcherApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButcherApp.Converter
{
    public interface IExport
    {
		void Export(string filePath,List<Rec> collection,IProgress<ProgressModel> progress);
    }
}
