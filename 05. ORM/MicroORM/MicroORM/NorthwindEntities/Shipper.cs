using LinqToDB.Mapping;

namespace MicroORM.Entities
{
    [Table("Shippers")]
    public class Shipper
    {
        [PrimaryKey]
        [Identity]
        [Column("ShipperID")]
        public int Id { get; set; }
        [Column("CompanyName")]
        public string Name;
    }
}