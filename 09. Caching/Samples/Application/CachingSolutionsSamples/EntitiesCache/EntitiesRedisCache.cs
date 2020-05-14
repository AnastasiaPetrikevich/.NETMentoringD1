using NorthwindLibrary;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.EntitiesCache
{
	public class EntitiesRedisCache : IEntitiesCache
	{
		private ConnectionMultiplexer redisConnection;
		private const string prefix = "Cache_";
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<Category>));

		public EntitiesRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

		public IEnumerable<T> Get<T>(string forUser)
		{
			var db = redisConnection.GetDatabase();
			byte[] s = db.StringGet(GetPrefix<T>(forUser));
			if (s == null)
				return null;

			return (IEnumerable<T>)serializer
				.ReadObject(new MemoryStream(s));

		}

		public void Set<T>(string forUser, IEnumerable<T> values)
		{
			var db = redisConnection.GetDatabase();
			var key = GetPrefix<T>(forUser);

			if (values == null)
			{
				db.StringSet(key, RedisValue.Null);
			}
			else
			{
				var stream = new MemoryStream();
				serializer.WriteObject(stream, values);
				db.StringSet(key, stream.ToArray());
			}
		}

		private string GetPrefix<T>(string forUser) => prefix + typeof(T).Name + forUser;
	}
}
