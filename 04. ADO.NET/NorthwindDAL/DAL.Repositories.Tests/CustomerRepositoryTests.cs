using DAL.Entities;
using DAL.Interfaces;
using DependencyResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Linq;

namespace DAL.Repositories.Tests
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private static ICustomerRepository repository;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            repository = Resolver.Kernel.Get<ICustomerRepository>();
        }

        [TestMethod]
        public void Get_CustomersID_Test()
        {
            var customersIDs = repository.GetIDs();

            Assert.IsNotNull(customersIDs, "Value can not be null");
            foreach (var id in customersIDs)
            {
                Console.WriteLine(id);
            }
        }

        [TestMethod]
        public void Get_ProductQuantities_Test()
        {
            string firstCustomer = repository.GetIDs().FirstOrDefault();

            Assert.IsNotNull(firstCustomer, "Customer table doesn't have any records");
            Console.WriteLine(firstCustomer);
            Console.WriteLine("------------");

            var productQuantities = repository.GetProductQuantities(firstCustomer);

            Assert.IsNotNull(productQuantities, "ProductQuantities mustn't be null");

            foreach (var pQ in productQuantities)
            {
                Console.WriteLine(pQ.ProductName + " - " + pQ.Quantity);
            }
        }
    }
}