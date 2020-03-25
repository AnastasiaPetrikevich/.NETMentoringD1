using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order GetOrderDetailByOrderID(int id);
        int Add(Order order);
        int Update(Order order);
        int Delete(int id);
        int SetInProgressStatus(int orderID);
        int SetDoneStatus(int orderID);
        IEnumerable<CustOrdersDetail> GetCustOrdersDetails(int orderID);
    }
}
