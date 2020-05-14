using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.EntitiesCache
{
	public interface IEntitiesCache
	{
		IEnumerable<T> Get<T>(string forUser);
		void Set<T>(string forUser, IEnumerable<T> values);
	}
}
