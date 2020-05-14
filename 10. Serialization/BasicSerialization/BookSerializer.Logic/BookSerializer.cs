using BookSerializer.Logic.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookSerializer.Logic
{
    public class BookSerializer
    {
		public void Serialize(string filePath, Catalog element)
		{
			var serializer = new XmlSerializer(typeof(Catalog));
			using (FileStream stream = new FileStream(filePath, FileMode.Create))
			{
				serializer.Serialize(stream, element);
			}
		}

		public Catalog Deserialize(string filePath)
		{
			var serializer = new XmlSerializer(typeof(Catalog));
			using (FileStream stream = new FileStream(filePath, FileMode.Open))
			{
				return (Catalog)serializer.Deserialize(stream);
			}
		}
	}
}
