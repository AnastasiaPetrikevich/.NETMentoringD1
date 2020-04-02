using LinqToDB.Mapping;

namespace MicroORM.Entities
{
    [Table("Order Details")]
    public class OrderDetails
    {
        [PrimaryKey(1)]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [PrimaryKey(2)]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column]
        public decimal UnitPrice { get; set; }
    }
}