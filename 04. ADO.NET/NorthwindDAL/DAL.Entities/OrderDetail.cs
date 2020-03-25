using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DAL.Entities
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }

    public static class OrderDetailHelpers
    {
        public static void Set(this OrderDetail order, DbDataReader reader)//avoid handling exception to save the time of writing
        {
            order.OrderID = (int)reader["OrderID"];
            order.ProductID = (int)reader["ProductID"];
            order.UnitPrice = (decimal)reader["UnitPrice"];
            order.Quantity = (short)reader["Quantity"];
            order.Discount = (float)reader["Discount"];
        }
    }
}
