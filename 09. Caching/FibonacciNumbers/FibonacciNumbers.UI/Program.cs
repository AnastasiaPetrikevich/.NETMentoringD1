using FibonacciNumbers.Logic;
using FibonacciNumbers.Logic.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciNumbers.UI
{
	class Program
	{
		static void Main(string[] args)
		{
			var cache = new Cache();
			var fibonacci = new FibonacciNumbersCaching(cache, new FibonacciNumbersGenerator());
			Console.WriteLine("Please, enter fibonacci number index: ");
			var index = BigInteger.Parse(Console.ReadLine());

			var result = fibonacci.Generate(index);

			Console.WriteLine($"Result: {result}");

			Console.ReadKey();
			
		}
	}
}
