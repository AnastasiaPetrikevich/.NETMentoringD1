using LinqToDB.Mapping;

namespace MicroORM.Entities
{
    [Table("EmployeeTerritories")]
    public class EmployeeTerritory
    {
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Column("TerritoryID")]
        public string TerritoryId { get; set; }

        [Association(ThisKey = "EmployeeId", OtherKey = "Id")]
        public Employee Employee;
        [Association(ThisKey = "TerritoryId", OtherKey = "Id")]
        public Territory Territory;
    }
}