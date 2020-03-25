using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DAL.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderDetail> Details { get; set; }
    }

    public static class OrderHelpers
    {
        public static void GetStatus(this Order order)
        {
            order.Status = ((order.OrderDate != null) ?
                                            ((order.ShippedDate == null) ? OrderStatus.InProgress : OrderStatus.Done)
                                            : OrderStatus.New);
        }

        public static void Set(this Order order, DbDataReader reader)
        {
            order.OrderID = (int)reader["OrderID"];
            order.CustomerID = (reader["CustomerID"] is System.DBNull) ? null : (string)reader["CustomerID"];
            order.EmployeeID = (reader["EmployeeID"] is System.DBNull) ? null : (int?)reader["EmployeeID"];
            order.OrderDate = (reader["OrderDate"] is System.DBNull) ? null : (DateTime?)reader["OrderDate"];
            order.RequiredDate = (reader["RequiredDate"] is System.DBNull) ? null : (DateTime?)reader["RequiredDate"];
            order.ShippedDate = (reader["ShippedDate"] is System.DBNull) ? null : (DateTime?)reader["ShippedDate"];
            order.ShipVia = (reader["ShipVia"] is System.DBNull) ? null : (int?)reader["ShipVia"];
            order.Freight = (reader["Freight"] is System.DBNull) ? null : (decimal?)reader["Freight"];
            order.ShipName = (reader["ShipName"] is System.DBNull) ? null : (string)reader["ShipName"];
            order.ShipAddress = (reader["ShipAddress"] is System.DBNull) ? null : (string)reader["ShipAddress"];
            order.ShipCity = (reader["ShipCity"] is System.DBNull) ? null : (string)reader["ShipCity"];
            order.ShipRegion = (reader["ShipRegion"] is System.DBNull) ? null : (string)reader["ShipRegion"];
            order.ShipPostalCode = (reader["ShipPostalCode"] is System.DBNull) ? null : (string)reader["ShipPostalCode"];
            order.ShipCountry = (reader["ShipCountry"] is System.DBNull) ? null : (string)reader["ShipCountry"];
        }
    }
}
