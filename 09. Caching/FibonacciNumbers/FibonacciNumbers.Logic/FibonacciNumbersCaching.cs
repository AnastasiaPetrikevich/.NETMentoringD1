using FibonacciNumbers.Logic.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciNumbers.Logic
{
	public class FibonacciNumbersCaching
	{
		private readonly ICache _cache;
		private readonly IFibonacciNumberGenerator _generator;

		public FibonacciNumbersCaching(ICache cache, IFibonacciNumberGenerator generator)
		{
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		}

		public BigInteger Generate(BigInteger index)
		{
			var result = _cache.Get(index.ToString());
			if (!result.HasValue)
			{
				result = _generator.Generate(index);
				_cache.Set(index.ToString(), result.Value);
			}

			return result.Value;
		}
	}
}
