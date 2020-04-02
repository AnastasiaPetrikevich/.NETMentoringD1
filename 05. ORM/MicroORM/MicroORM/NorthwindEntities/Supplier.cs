using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;

namespace MicroORM.Entities
{
    [Table("Suppliers")]
    public class Supplier
    {
        [PrimaryKey]
        [Identity]
        [Column("SupplierID")]
        public int Id { get; set; }
        [Column]
        public string CompanyName { get; set; }
    }
}