using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DIContainer.UI.TestData;

namespace DIContainer.UI
{
	class Program
	{
		static void Main(string[] args)
		{
			var container = new Container();
			container.AddAssembly(Assembly.GetExecutingAssembly());
			var customerBLL = container.CreateInstance(typeof(CustomerBLL));
			Console.WriteLine(customerBLL.GetType());

			container.AddType(typeof(CustomerBLL));
			container.AddType(typeof(Logger));
			container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
			customerBLL = container.CreateInstance(typeof(CustomerBLL));
			Console.WriteLine(customerBLL.GetType());


			container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
			var customerDAL = container.CreateInstance<ICustomerDAL>();
			Console.WriteLine(customerDAL.GetType());

			Console.ReadKey();
		}
	}
}
