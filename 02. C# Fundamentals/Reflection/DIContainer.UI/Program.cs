using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DIContainer.Models;

namespace DIContainer.UI
{
	class Program
	{
		static void Main(string[] args)
		{
			DemoWithTypeAddingFromAssembly();
			Console.WriteLine(new String('-', 100));
			DemoWithManualTypeAdding();
			Console.ReadLine();

			Console.ReadKey();
		}

		private static void DemoWithTypeAddingFromAssembly()
		{
			var container = new Container();
			container.AddAssembly(Assembly.Load(Assembly.GetExecutingAssembly().GetReferencedAssemblies()
				.SingleOrDefault(a => a.Name == "DIContainer.Models").FullName));
			WriteOutputWithCasting(container);
			WriteOutputWithGeneric(container);
		}

		private static void DemoWithManualTypeAdding()
		{
			var container = new Container();
			container.AddType(typeof(Logger));
			container.AddType(typeof(Logger2));
			container.AddType(typeof(CustomerBLL));
			container.AddType(typeof(CustomerBLL2));
			container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

			WriteOutputWithCasting(container);
			WriteOutputWithGeneric(container);
		}

		private static void WriteOutputWithCasting(Container container)
		{
			var customerBLL = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
			Console.WriteLine($"{customerBLL.CustomerDAL.CustomerMessage} | {customerBLL.Logger.LoggerMessage} | {customerBLL.Logger2.LoggerMessage}");
			
			var customerBLL2 = (CustomerBLL2)container.CreateInstance(typeof(CustomerBLL2));
			Console.WriteLine($"{customerBLL2.CustomerDAL.CustomerMessage} | {customerBLL2.Logger.LoggerMessage} | {customerBLL2.Logger2.LoggerMessage}");
		}

		private static void WriteOutputWithGeneric(Container container)
		{
			var customerBLL_ = container.CreateInstance<CustomerBLL>();
			Console.WriteLine($"{customerBLL_.CustomerDAL.CustomerMessage} | {customerBLL_.Logger.LoggerMessage} | {customerBLL_.Logger2.LoggerMessage}");

			var customerBLL2_ = container.CreateInstance<CustomerBLL2>();
			Console.WriteLine($"{customerBLL2_.CustomerDAL.CustomerMessage} | {customerBLL2_.Logger.LoggerMessage} | {customerBLL2_.Logger2.LoggerMessage}");
		}
	}
}
