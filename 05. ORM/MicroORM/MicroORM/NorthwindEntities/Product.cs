using System.Collections.Generic;
using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlQuery;

namespace MicroORM.Entities
{
    [Table("Products")]
    public class Product
    {
        [PrimaryKey]
        [Identity]
        [Column("ProductID")]
        public int Id { get; set; }
        [Column("ProductName")]
        public string Name { get; set; }
        [Column]
        public int SupplierId { get; set; }
        [Column("CategoryID")]
        public int CategoryId { get; set; }
        [Column]
        public decimal UnitPrice { get; set; }

        [Association(ThisKey = "SupplierId", OtherKey = "Id")]
        public Supplier Supplier;
        [Association(ThisKey = "CategoryId", OtherKey = "Id")]
        public Category Category;
    }
}