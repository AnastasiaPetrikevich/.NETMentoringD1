using MicroORM.Entities;
using LinqToDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.Data;

namespace MicroORM
{
	[TestClass]
	public class Task3
	{
		[TestMethod]
		public void AddNewEmployeeWithTerritoriesListHeIsResponsibleFor()
		{
			using (var connection = new Northwind())
			{
				var id = (int)(decimal)connection.InsertWithIdentity(new Employee() { FirstName = "Mia", LastName = "Grey" });

				int territoryCount = 2;
				var territories = connection.Territories.Select(x => x.Id).Take(territoryCount).ToArray();
				for (int i = 0; i < territoryCount; i++)
				{
					connection.Insert(new EmployeeTerritory() { EmployeeId = id, TerritoryId = territories[i] });
				}
			}
		}

		[TestMethod]
		public void MoveProductsToTheOtherCategory()
		{
			using (var connection = new Northwind())
			{
				var category = connection.Categories.First(c => c.Name == "Seafood");
				var prodroducts = connection.Products.First(p => p.CategoryId != category.Id);
				prodroducts.CategoryId = category.Id;

				connection.Update(prodroducts);
			}
		}

		[TestMethod]
		public void ScopeProductsAdding()
		{
			using (var connection = new Northwind())
			{
				var products = new List<Product>()
								   {
									   new Product()
										   {
											   Name = "Product1",
											   Category = new Category { Name = "Category1" },
											   Supplier = new Supplier { CompanyName = "Supplier1" }
										   },
									   new Product()
										   {
											   Name = "Product2",
											   Category = new Category { Name = "Category2" },
											   Supplier = new Supplier { CompanyName = "Supplier2" }
										   },
									   new Product()
										   {
											   Name = "Product3",
											   Category = new Category { Name = "Category1" },
											   Supplier = new Supplier { CompanyName = "Supplier2" }
										   }
								   };

				connection.BulkCopy(new BulkCopyOptions { BulkCopyType = BulkCopyType.ProviderSpecific },products);
			}
		}

		[TestMethod]
		public void ReplaceProducts()
		{
			using (var connection = new Northwind())
			{
				var incompleteOrderIds = connection.Orders.Where(o => o.ShippedDate == null).Select(o => o.Id).ToList();
				var incompleteOrders = connection.OrderDetails.ToList().Where(d => incompleteOrderIds.Any(o => o == d.OrderId)).OrderByDescending(io => io.ProductId);
				foreach (var incompleteOrder in incompleteOrders)
				{
					var newProduct = incompleteOrder.ProductId + 1;
					if (!connection.Products.Any(p => p.Id == newProduct))
					{
						newProduct = connection.Products.First().Id;
					}

					var product = connection.Products.First(p => p.Id == newProduct);
					var newPrice = product.UnitPrice;
					connection.OrderDetails.Where(od => od.OrderId == incompleteOrder.OrderId && od.ProductId == incompleteOrder.ProductId)
						.Update(od => new OrderDetails()
						{
							ProductId = newProduct,
							UnitPrice = newPrice
						});
				}
			}
		}
	}
}
