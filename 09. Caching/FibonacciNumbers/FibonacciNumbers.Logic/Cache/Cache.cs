using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciNumbers.Logic.Cache
{
	public class Cache : ICache
	{
		private const string prefix = "fibonacci_";

		public BigInteger? Get(string key)
		{
			ObjectCache cache = MemoryCache.Default;
			return cache.Get(GetKey(key)) as BigInteger?;
		}

		public void Set(string key, BigInteger number)
		{
			ObjectCache cache = MemoryCache.Default;
			cache.Set(GetKey(key), number, ObjectCache.InfiniteAbsoluteExpiration);
		}

		private string GetKey(string value) => prefix + value;

	}
}
