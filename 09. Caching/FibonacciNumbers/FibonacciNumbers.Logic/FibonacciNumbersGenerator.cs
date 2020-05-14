using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciNumbers.Logic
{
    public class FibonacciNumbersGenerator : IFibonacciNumberGenerator
    {
		public BigInteger Generate(BigInteger index)
		{
			if (index == 0)
			{
				return BigInteger.Zero;
			}

			if (index <= 2)
			{
				return BigInteger.One;
			}

			var previous = BigInteger.One;
			var current = BigInteger.One;

			for (int i = 2; i <= index; i++)
			{				
				var next = previous + current;
				previous = current;
				current = next;
			}

			return current;
		}
	}
}
