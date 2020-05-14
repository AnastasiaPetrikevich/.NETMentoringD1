using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciNumbers.Logic
{
	public interface IFibonacciNumberGenerator
	{
		BigInteger Generate(BigInteger index);
	}
}
