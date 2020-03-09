using DIContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer.UI.TestData
{
	[Export]
	public class CustomerBLL
	{
		public CustomerBLL(ICustomerDAL customerDAL, Logger logger)
		{

		}
	}
}
