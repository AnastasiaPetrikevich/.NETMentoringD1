using LinqToDB.Mapping;

namespace MicroORM.Entities
{
    [Table("Region")]
    public class Region
    {
        [PrimaryKey]
        [Identity]
        [Column("RegionID")]
        public int Id { get; set; }
        [Column("RegionDescription")]
        public string Description { get; set; }
    }
}
