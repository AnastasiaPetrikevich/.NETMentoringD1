using DIContainer.Attributes;

namespace DIContainer.Models
{
	[Export(typeof(ICustomerDAL))]
	public class CustomerDAL : ICustomerDAL
	{
		public string CustomerMessage  => "Customer DAL"; 
	}
}
