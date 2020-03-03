using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSymbol
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Please, enter string: ");

			while (Console.ReadKey(true).Key != ConsoleKey.Escape)
			{
				string sourceString = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(sourceString))
				{
					Console.WriteLine("You entered string is null, empty or white space.");
					continue;
				}
				
				Console.WriteLine($"First character: '{sourceString[0]}'.");
				
			}

		}
	}
}
