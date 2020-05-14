using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookSerializer.Logic.Entities
{
	public class Book
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("isbn")]
		public string Isbn { get; set; }

		[XmlElement("author")]
		public string Author { get; set; }

		[XmlElement("title")]
		public string Title { get; set; }

		[XmlElement("genre")]
		public Genre Genre { get; set; }

		[XmlElement("publisher")]
		public string Publisher { get; set; }

		[XmlIgnore]
		public DateTime PublishDate { get; set; }

		[XmlElement("publish_date")]
		public string PublishDateSerializable
		{
			get  => PublishDate.ToString("yyyy-MM-dd");			
			set => PublishDate = DateTime.Parse(value);			
		}

		[XmlElement("description")]
		public string Description { get; set; }

		[XmlIgnore]
		public DateTime RegistrationDate { get; set; }

		[XmlElement("registration_date")]
		public string RegistrationDateSerializable
		{
			get => RegistrationDate.ToString("yyyy-MM-dd");

			set => RegistrationDate = DateTime.Parse(value);
		}
	}
}
