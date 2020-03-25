using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DAL.Entities
{
    public class CustOrdersDetail
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public int Discount { get; set; }
        public decimal ExtendedPrice { get; set; }
    }

    public static class CustOrdersDetailHelpers
    {
        public static void Set(this CustOrdersDetail order, DbDataReader reader)
        {
            order.ProductName = (string)reader["ProductName"];
            order.UnitPrice = (decimal)reader["UnitPrice"];
            order.Quantity = (short)reader["Quantity"];
            order.Discount = (int)reader["Discount"];
            order.ExtendedPrice = (decimal)reader["ExtendedPrice"];
        }
    }
}
