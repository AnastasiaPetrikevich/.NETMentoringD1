using LinqToDB;
using LinqToDB.Data;
using MicroORM.Entities;

namespace MicroORM
{
    public class Northwind : DataConnection
    {
        public Northwind() : base("Northwind")
        { }

        public ITable<Product> Products { get { return GetTable<Product>(); } }
        public ITable<Category> Categories { get { return GetTable<Category>(); } }
        public ITable<Employee> Employees { get { return GetTable<Employee>(); } }
        public ITable<EmployeeTerritory> EmployeeTerritories
        {
            get
            {
                return GetTable<EmployeeTerritory>()
                    .LoadWith(et => et.Employee)
                    .LoadWith(et => et.Territory)
                    .LoadWith(et => et.Territory.Region);
            }
        }
        public ITable<Order> Orders { get { return GetTable<Order>().LoadWith(o => o.Employee).LoadWith(o => o.Shipper); } }
        public ITable<OrderDetails> OrderDetails{ get { return GetTable<OrderDetails>(); } }
        public ITable<Region> Regions { get { return GetTable<Region>(); } }
        public ITable<Shipper> Shippers { get { return GetTable<Shipper>(); } }
        public ITable<Supplier> Suppliers { get { return GetTable<Supplier>(); } }
        public ITable<Territory> Territories { get { return GetTable<Territory>(); } }
    }
}