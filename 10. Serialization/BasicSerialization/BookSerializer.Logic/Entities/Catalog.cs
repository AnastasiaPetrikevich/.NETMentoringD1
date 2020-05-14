using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookSerializer.Logic.Entities
{
	[XmlRoot("catalog", Namespace = @"http://library.by/catalog")]
	public class Catalog
	{
		[XmlElement("book")]
		public List<Book> Books { get; set; }

		[XmlIgnore]
		public DateTime Date { get; set; }

		[XmlAttribute("date")]
		public string DateSerializable
		{
			get =>	Date.ToString("yyyy-MM-dd");
			set =>	Date = DateTime.Parse(value);
		}
	}
}
