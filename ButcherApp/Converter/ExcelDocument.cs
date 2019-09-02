using ButcherApp.Models;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ButcherApp.Converter
{
	public class ExcelDocument : IExport
	{
		public void Export(string filePath,List<Rec> collection)
		{	
			Rec rec = new Rec();
			string sheetName = Path.GetFileNameWithoutExtension(filePath);
			XSSFWorkbook xSSF = new XSSFWorkbook();
			ISheet sheet = xSSF.CreateSheet(sheetName);
			var propertiesName = rec.GetType().GetProperties();

			var headerRow = sheet.CreateRow(0);
			for (int i = 0; i < propertiesName.Length; i++)
			{
				var cell = headerRow.CreateCell(i);
				cell.SetCellValue(propertiesName[i].Name);
			}
			for (int i = 0; i < collection.Count; i++)
			{
				var rowIndex = i + 1;
				var row = sheet.CreateRow(rowIndex);

				for (int j = 0; j < propertiesName.Length; j++)
				{
					
					var o = collection[i];
					var value = o.GetType().GetProperty(propertiesName[j].Name).GetValue(o, null).ToString();
					CellType cellType = propertiesName[j].PropertyType.Name =="String" ? CellType.String : CellType.Numeric;
					var cell = row.CreateCell(j);
					cell.SetCellValue(value);
					cell.SetCellType(cellType);
				}
			}


			FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
			xSSF.Write(file);
			file.Dispose();
			file.Close();
		}

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
