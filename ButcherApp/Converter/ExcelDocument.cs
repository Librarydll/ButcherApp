using ButcherApp.Core;
using ButcherApp.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ButcherApp.Converter
{
	public class ExcelDocument : IExport
	{
		EcxelSettingHelper settingHelper;
		public ExcelDocument()
		{
			settingHelper = new EcxelSettingHelper();
		}
		public async Task Export(string filePath, List<Rec> collection, IProgress<ProgressModel> progress) => await Task.Run(() =>
		   {
			   Rec rec = new Rec();
			   string sheetName = Path.GetFileNameWithoutExtension(filePath);
			   XSSFWorkbook xSSF = new XSSFWorkbook();
			   ISheet sheet = xSSF.CreateSheet(sheetName);
			   var propertiesName = rec.GetType().GetProperties();
			   ProgressModel model = new ProgressModel();

			   var requiredProps = settingHelper.GetSettingNames();
			   var headerRow = sheet.CreateRow(0);
			   for (int i = 0,j=0; i < propertiesName.Length; i++)
			   {
				   if (!requiredProps.Any(x => x.Equals(propertiesName[i].Name))) continue;

				   var cell = headerRow.CreateCell(j);
				   cell.SetCellValue(propertiesName[i].Name);
				   j++;
			   }

			   for (int i = 0; i < collection.Count; i++)
			   {
				   var rowIndex = i + 1;
				   var row = sheet.CreateRow(rowIndex);
				   model.Percentage = i;
				   progress.Report(model);
				   var o = collection[i];
				   for (int j = 0,k=0; j < propertiesName.Length; j++)
				   {
					   if (!requiredProps.Any(x => x.Equals(propertiesName[j].Name))) continue;

					   var value = o.GetType().GetProperty(propertiesName[j].Name).GetValue(o, null).ToString();
					  CellType cellType = propertiesName[j].PropertyType.Name == "String" ? CellType.String : (propertiesName[j].PropertyType.Name == "Nullable`1" ? CellType.String : CellType.Numeric);
					   var cell = row.CreateCell(k);
					   k++;
					   if (cellType == CellType.Numeric)
					   {
						   var result = double.Parse(value);
						   cell.SetCellValue(result);
					   }
					   else
					   {
						   cell.SetCellValue(value);
					   }
					   cell.SetCellType(cellType);

				   }
			   }
			   model.Percentage = 0;
			   progress.Report(model);
			   FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
			   xSSF.Write(file);
			   file.Dispose();
			   file.Close();
			   Process.Start(filePath);
		   });


		private  DataTable CreateDataTable<T>(IEnumerable<T> list)
		{
			Type type = typeof(T);
			var properties = type.GetProperties();
			DataTable dataTable = new DataTable();
			foreach (var info in properties)
			{
				var temp = Nullable.GetUnderlyingType(info.PropertyType);
				var temp2 = info.PropertyType;
				dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
			}
			foreach (T entity in list)
			{
				object[] values = new object[properties.Length];
				for (int i = 0; i < properties.Length; i++)
				{
					values[i] = properties[i].GetValue(entity);
				}
				dataTable.Rows.Add(values);
			}
			return dataTable;
		}
	}
}
