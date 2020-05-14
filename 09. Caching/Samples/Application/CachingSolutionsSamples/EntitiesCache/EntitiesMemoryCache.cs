using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.EntitiesCache
{
	public class EntitiesMemoryCache : IEntitiesCache
	{
		ObjectCache cache = MemoryCache.Default;
		private const string prefix = "Cache_";
		private readonly string connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
		private readonly string sqlMonitor;

		public EntitiesMemoryCache(){}

		public EntitiesMemoryCache(string sqlMonitor)
		{
			this.sqlMonitor = sqlMonitor;
		}

		public IEnumerable<T> Get<T>(string forUser) => (IEnumerable<T>)cache.Get(GetPrefix<T>(forUser));


		public void Set<T>(string forUser, IEnumerable<T> values)
		{
			SqlDependency.Start(connectionString);
			cache.Set(GetPrefix<T>(forUser), values, GetCacheItemPolicy(sqlMonitor));
		}

		private string GetPrefix<T>(string forUser) => prefix + typeof(T).Name + forUser;

		private CacheItemPolicy GetCacheItemPolicy(string sqlMonitor)
		{
			return new CacheItemPolicy
			{
				ChangeMonitors = { GetSqlMonitor(sqlMonitor) }
			};
		}

		private SqlChangeMonitor GetSqlMonitor(string query)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var sqlCommand = new SqlCommand(query, connection);
				var monitor = new SqlChangeMonitor(new SqlDependency(sqlCommand));
				sqlCommand.ExecuteNonQuery();
				return monitor;
			}
		}
	}
}
