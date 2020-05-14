using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciNumbers.Logic.Cache
{
	public interface ICache
	{
		BigInteger? Get(string key);

		void Set(string key, BigInteger number);
	}
}
