using System;
using LinqToDB.Mapping;

namespace MicroORM.Entities
{
    [Table("Orders")]
    public class Order
    {
        [PrimaryKey]
        [Identity]
        [Column("OrderID")]
        public int Id { get; set; }
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Column]
        public int ShipVia { get; set; }
        [Column]
        public DateTime ShippedDate { get; set; }

        [Association(ThisKey = "EmployeeId", OtherKey = "Id")]
        public Employee Employee;
        [Association(ThisKey = "ShipVia", OtherKey = "Id")]
        public Shipper Shipper;
    }
}