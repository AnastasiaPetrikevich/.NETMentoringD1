using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DAL.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }

    public static class ProductHelpers
    {
        public static void Set(this Product order, DbDataReader reader)//avoid handling exception to save the time of writing
        {
            order.ProductID = (int)reader["ProductID"];
            order.ProductName = (string)reader["ProductName"];
            order.SupplierID = (reader["SupplierID"] is System.DBNull) ? null : (int?)reader["SupplierID"];
            order.CategoryID = (reader["CategoryID"] is System.DBNull) ? null : (int?)reader["CategoryID"];
            order.QuantityPerUnit = (reader["QuantityPerUnit"] is System.DBNull) ? null : (string)reader["QuantityPerUnit"];
            order.UnitPrice = (reader["UnitPrice"] is System.DBNull) ? null : (decimal?)reader["UnitPrice"];
            order.UnitsInStock = (reader["UnitsInStock"] is System.DBNull) ? null : (short?)reader["UnitsInStock"];
            order.UnitsOnOrder = (reader["UnitsOnOrder"] is System.DBNull) ? null : (short?)reader["UnitsOnOrder"];
            order.ReorderLevel = (reader["ReorderLevel"] is System.DBNull) ? null : (short?)reader["ReorderLevel"];
            order.Discontinued = (bool)reader["Discontinued"];
        }
    }
}
