using LinqToDB.Mapping;

namespace MicroORM.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [PrimaryKey]
        [Identity]
        [Column("EmployeeID")]
        public int Id { get; set; }
        [Column]
        public string FirstName { get; set; }
        [Column]
        public string LastName { get; set; }
    }
}