using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<string> GetIDs();
        IEnumerable<ProductQuantity> GetProductQuantities(string customerID);
    }
}
