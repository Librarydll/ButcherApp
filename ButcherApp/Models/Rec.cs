using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ButcherApp.Models
{
	public class Rec
	{
		[XmlAttribute(AttributeName = "Key")]
		public string Key { get; set; }
		[XmlAttribute(AttributeName = "Flag")]
		public string Flag { get; set; }
		[XmlAttribute(AttributeName = "Print")]
		public int Print { get; set; }

		[XmlElement(ElementName = "HistName")]
		public string HistName { get; set; }

		[XmlElement(ElementName = "Date")]
		public string Date { get; set; }

		[XmlElement(ElementName ="Time")]
		public string Time { get; set; }

		[XmlElement(ElementName = "HeadName")]
		public string HeadName { get; set; }

		[XmlElement(ElementName = "HeadSide")]
		public int HeadSide { get; set; }

		[XmlElement(ElementName = "HeadProgres")]
		public int HeadProgres { get; set; }

		[XmlElement(ElementName = "Tare")]
		public int Tare { get; set; }

		[XmlElement(ElementName = "Net")]
		public double Net { get; set; }

		[XmlElement(ElementName = "Gross")]
		public double Gross { get; set; }

		[XmlElement(ElementName = "Flow")]
		public double Flow { get; set; }

		[XmlElement(ElementName = "ProductCode")]
		public string ProductCode { get; set; }

		[XmlElement(ElementName = "ProductName")]
		public string ProductName { get; set; }

		[XmlElement(ElementName = "ProductProgres")]
		public int ProductProgres { get; set; }

		[XmlElement(ElementName = "ProductTot")]
		public double ProductTot { get; set; }

		[XmlElement(ElementName = "Client")]
		public string Client { get; set; }

		[XmlElement(ElementName = "Expiry")]
		public string Expiry { get; set; }

		[XmlElement(ElementName = "Note0")]
		public string Note0 { get; set; }

		[XmlElement(ElementName = "Note1")]
		public string Note1 { get; set; }

		[XmlElement(ElementName = "Note2")]
		public string Note2 { get; set; }

		[XmlElement(ElementName = "Note3")]
		public string Note3 { get; set; }

		[XmlElement(ElementName = "Note4")]
		public string Note4 { get; set; }

		[XmlElement(ElementName = "Note5")]
		public string Note5 { get; set; }

		[XmlElement(ElementName = "Note6")]
		public string Note6 { get; set; }

		[XmlElement(ElementName = "FillerProgres")]
		public string FillerProgres { get; set; }

		[XmlElement(ElementName = "YearCode")]
		public string YearCode { get; set; }

		[XmlElement(ElementName = "DayCode")]
		public string DayCode { get; set; }

		[XmlElement(ElementName = "PlantCode")]
		public int PlantCode { get; set; }

		[XmlElement(ElementName = "Plant")]
		public string Plant { get; set; }

		[XmlElement(ElementName = "ShiftCode")]
		public string ShiftCode { get; set; }

		[XmlElement(ElementName = "CapSter")]
		public string CapSter { get; set; }

		[XmlElement(ElementName = "Container")]
		public int Container { get; set; }

		[XmlElement(ElementName = "DecimalSymbol")]
		public string DecimalSymbol { get; set; }

		[XmlElement(ElementName = "BarCode")]
		public string  BarCode { get; set; }


	}
}
