using LinqToDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MicroORM
{
    [TestClass]
    public class Task2
    {
        [TestMethod]
        public void ListOfTheProductsWithCategoryAndSuppliers()
        {
            using (var connection = new Northwind())
            {
                foreach (var products in connection.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier))
                {
                    Console.WriteLine($"Name: {products.Name}, Category: {products.Category.Name}, Supplier: {products.Supplier.CompanyName}");
                }
            }
        }

        [TestMethod]
        public void ListOfEmployersWithRegionTheyResponseFor()
        {
            using (var connection = new Northwind())
            {
                foreach (var territory in connection.EmployeeTerritories
                    .Select(t => new
                    {
                        Name = t.Employee.FirstName,
                        Region = t.Territory.Region.Description
                    })
                    .Distinct())
                {
                    Console.WriteLine($"Name: {territory.Name}, Region: {territory.Region}");
                }
            }
        }

        [TestMethod]
        public void NumberOfTheEmployersByRegions()
        {
            using (var connection = new Northwind())
            {
                foreach (var territory in connection.EmployeeTerritories
                    .Select(t => new
                    {
                        Employee = t.EmployeeId,
                        Region = t.Territory.Region.Description
                    })
                    .Distinct()
                    .GroupBy(t => t.Region))
                {
                    Console.WriteLine($"Region: {territory.Key}, Employee count: {territory.Count()}");
                }
            }
        }

        [TestMethod]
        public void ListOfEmployersWithShippersTheyHaveBeenWorkingWith()
        {
            using (var connection = new Northwind())
            {
				foreach (var order in connection.Orders
					.Select(o => new
					{
						Employee = o.Employee.FirstName,
                        Shipper = o.Shipper.Name
                    })
                    .Distinct()
                    .GroupBy(o => o.Employee))
                {
                    Console.WriteLine($"Employee: {order.Key}");
                    Console.WriteLine("Shippers:");
                    foreach (var employee in order)
                    {
                        Console.WriteLine($"{employee.Shipper}");
                    }
                }
            }
        }
    }
}
