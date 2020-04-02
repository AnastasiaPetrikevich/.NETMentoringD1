using LinqToDB.Mapping;

namespace MicroORM.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Column("CategoryID"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column("CategoryName")]
        public string Name { get; set; }

        [Column]
        public string Description { get; set; }
    }
}