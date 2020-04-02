using LinqToDB.Mapping;

namespace MicroORM.Entities
{
    [Table("Territories")]
    public class Territory
    {
        [PrimaryKey]
        [Identity]
        [Column("TerritoryID")]
        public string Id { get; set; }
        [Column]
        public int RegionId { get; set; }
        [Column("TerritoryDescription")]
        public string Description { get; set; }

        [Association(ThisKey = "RegionId", OtherKey = "Id")]
        public Region Region;
    }
}