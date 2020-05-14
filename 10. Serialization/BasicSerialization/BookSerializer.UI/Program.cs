using BookSerializer.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSerializer.UI
{
	class Program
	{
		private const string bookXml = @"D:\.NETMentoringD1\10. Serialization\BasicSerialization\books.xml";
		private const string newBookXml = @"D:\.NETMentoringD1\10. Serialization\BasicSerialization\newBooks.xml";

		static void Main(string[] args)
		{
			var bookSerializer = new BookSerializer.Logic.BookSerializer();
			Catalog result = bookSerializer.Deserialize(bookXml);
			bookSerializer.Serialize(newBookXml, result);
		}
	}
}
