using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.EntitiesCache
{
	[TestClass]
	public class EntitiesCacheTests
	{
		public void Test_Categories_MemoryCache()
		{
			var entitiesManager = new EntitiesManager<Category>(new EntitiesMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entitiesManager.GetEntities().Count());
				Thread.Sleep(100);
			}
		}

		public void Test_Orders_MemoryCache()
		{
			var entitiesManager = new EntitiesManager<Order>(new EntitiesMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entitiesManager.GetEntities().Count());
				Thread.Sleep(100);
			}
		}

		public void Test_Suppliers_MemoryCache()
		{
			var entitiesManager = new EntitiesManager<Supplier>(new EntitiesMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entitiesManager.GetEntities().Count());
				Thread.Sleep(100);
			}
		}

		public void Test_Categories_MemoryCache_SqlMonitor()
		{
			var sqlMonitor = "SELECT [CategoryID] FROM [dbo].[Categories]";
			var entitiesManager = new EntitiesManager<Category>(new EntitiesMemoryCache(sqlMonitor));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entitiesManager.GetEntities().Count());
				Thread.Sleep(100);
			}
		}

		public void Test_Orders_MemoryCache_SqlMonitor()
		{
			var sqlMonitor = "SELECT OrderID, OrderDate FROM [dbo].[Orders]";
			var entitiesManager = new EntitiesManager<Order>(new EntitiesMemoryCache(sqlMonitor));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entitiesManager.GetEntities().Count());
				Thread.Sleep(100);
			}
		}

		public void Test_Suppliers_MemoryCache_SqlMonitor()
		{
			var sqlMonitor = "SELECT SupplierID, CompanyName, ContactName, FROM [dbo].[Suppliers]";
			var entitiesManager = new EntitiesManager<Supplier>(new EntitiesMemoryCache(sqlMonitor));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entitiesManager.GetEntities().Count());
				Thread.Sleep(100);
			}
		}


		public void Test_Categories_RedisCache()
		{
			var entitiesManager = new EntitiesManager<Category>(new EntitiesRedisCache("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entitiesManager.GetEntities().Count());
				Thread.Sleep(100);
			}
		}

		public void Test_Orders_RedisCache()
		{
			var entitiesManager = new EntitiesManager<Order>(new EntitiesRedisCache("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(entitiesManager.GetEntities().Count());
				Thread.Sleep(100);
			}
		}
	}
}
