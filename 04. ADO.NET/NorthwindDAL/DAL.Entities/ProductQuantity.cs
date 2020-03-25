using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DAL.Entities
{
    public class ProductQuantity
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    public static class ProductQuantityHelpers
    {
        public static void Set(this ProductQuantity order, DbDataReader reader)//avoid handling exception to save the time of writing
        {
            order.ProductName = (string)reader["ProductName"];
            order.Quantity = (int)reader["Total"];
        }
    }
}
