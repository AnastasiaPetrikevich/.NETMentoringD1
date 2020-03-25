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
    public class OrderRepositoryTests
    {
        private static IOrderRepository repository;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
             repository = Resolver.Kernel.Get<IOrderRepository>();
        }

        [TestMethod]
        public void Get_AllOrders_Test()
        {
            var orders = repository.GetAll();

            foreach(var order in orders)
            {
                Assert.IsNull(order.Details, "Details should be empty");
            }
        }

        [TestMethod]
        public void Get_OrderDetail_By_OrderID_Test()
        {
            Order firstOrder = repository.GetAll().FirstOrDefault();

            Assert.IsNotNull(firstOrder, "Order table doesn't have any records");

            Order fullOrder = repository.GetOrderDetailByOrderID(firstOrder.OrderID);

            Assert.IsNotNull(fullOrder, "Order mustn't be null");
            Assert.IsNotNull(fullOrder.Details, "Order.Detail mustn't be null");

            Console.WriteLine("Details count is {0}",fullOrder.Details.Count());

            foreach (var detail in fullOrder.Details)
            {
                Assert.IsNotNull(detail.Product, "Order.Detail.Product mustn't be null");
                Assert.AreEqual(detail.Order, fullOrder, "Detail should reference to parent order");
            }
        }

        [TestMethod]
        public void Add_NewOrder_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34
			};
            var newId = repository.Add(newOrder);

            Assert.IsTrue(newId > 0, "Object are not created");
        }

        #region Update
        [TestMethod]
        public void Update_NewOrder_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34
			};
            newOrder.OrderID = repository.Add(newOrder);

            var newFreight = newOrder.Freight += 1;
            var affected = repository.Update(newOrder);
            Assert.AreEqual(affected, 1, "Order are not updated");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(newFreight, fullOrder.Freight, "Order are not updated");
        }

        [TestMethod]
        public void Update_InProgress_Order_Test()
        {
            var oldFreight = (decimal)15.34;
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = oldFreight,
				OrderDate = DateTime.Now
			};
            newOrder.OrderID = repository.Add(newOrder);

            var newFreight = newOrder.Freight += 1;
            var affected = repository.Update(newOrder);
            Assert.AreEqual(affected, 0, "InProgress order should not be updated");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(oldFreight, fullOrder.Freight, "InProgress order should not be updated");
        }

        [TestMethod]
        public void Update_Done_Order_Test()
        {
            var oldFreight = (decimal)15.34;
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = oldFreight,
				OrderDate = DateTime.Now,
				ShippedDate = DateTime.Now
			};
            newOrder.OrderID = repository.Add(newOrder);

            var newFreight = newOrder.Freight += 1;
            var affected = repository.Update(newOrder);
            Assert.AreEqual(affected, 0, "Done order should not be updated");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(oldFreight, fullOrder.Freight, "Done order should not be updated");
        }
        #endregion

        #region Delete
        [TestMethod]
        public void Delete_New_Order_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.Delete(newOrder.OrderID);
            Assert.AreEqual(affected, 1, "Order are not deleted");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.IsNull(fullOrder, "Order are not deleted");
        }

        [TestMethod]
        public void Delete_InProgress_Order_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34,
				OrderDate = DateTime.Now
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.Delete(newOrder.OrderID);
            Assert.AreEqual(affected, 1, "Order are not deleted");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.IsNull(fullOrder, "Order are not deleted");
        }

        [TestMethod]
        public void Delete_Done_Order_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34,
				OrderDate = DateTime.Now,
				ShippedDate = DateTime.Now
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.Delete(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order should not be deleted");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.IsNotNull(fullOrder, "Order should not be deleted");
        }
        #endregion

        #region Set_InProgress_Status
        [TestMethod]
        public void Set_InProgress_Status_NewOrder_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.SetInProgressStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 1, "Order are not set to InProgress");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(fullOrder.Status, OrderStatus.InProgress, "Order are not set to InProgress");
        }

        [TestMethod]
        public void Set_InProgress_Status_InProgress_Order_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34,
				OrderDate = DateTime.Now
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.SetInProgressStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order are not set to InProgress");
        }

        [TestMethod]
        public void Set_InProgress_Status_Done_Order_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34,
				OrderDate = DateTime.Now,
				ShippedDate = DateTime.Now
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.SetInProgressStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order are not set to InProgress");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(fullOrder.Status, OrderStatus.Done, "Order are not set to InProgress");
        }
        #endregion

        #region SetDoneStatus
        [TestMethod]
        public void Set_Done_Status_NewOrder_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.SetDoneStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order are not set to Done");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(fullOrder.Status, OrderStatus.New, "Order are not set to Done");
        }

        [TestMethod]
        public void Set_DoneStatus_InProgress_Order_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34,
				OrderDate = DateTime.Now
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.SetDoneStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 1, "Order are not set to Done");

            Order fullOrder = repository.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(fullOrder.Status, OrderStatus.Done, "Order are not set to Done");
        }

        [TestMethod]
        public void Set_Done_Status_Done_Order_Test()
        {
            var newOrder = new Order()
			{
				OrderID = 1,
				Freight = (decimal)15.34,
				OrderDate = DateTime.Now,
				ShippedDate = DateTime.Now
			};
            newOrder.OrderID = repository.Add(newOrder);

            var affected = repository.SetDoneStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order are not set to Done");
        }
        #endregion

        [TestMethod]
        public void Get_Cust_OrdersDetails_Test()
        {
            Order firstOrder = repository.GetAll().FirstOrDefault();

            Assert.IsNotNull(firstOrder, "Orders table doesn't have any records");
            Console.WriteLine(firstOrder.OrderID);
            Console.WriteLine("------------");

            var custOrdersDetails = repository.GetCustOrdersDetails(firstOrder.OrderID);

            Assert.IsNotNull(custOrdersDetails, "CustOrdersDetails mustn't be null");

            foreach (var cOD in custOrdersDetails)
            {
                Console.WriteLine(cOD.ProductName);
                Console.WriteLine("-- UnitPrice = " + cOD.UnitPrice);
                Console.WriteLine("-- Quantity = " + cOD.Quantity);
                Console.WriteLine("-- Discount = " + cOD.Discount);
                Console.WriteLine("-- ExtendedPrice = " + cOD.ExtendedPrice);
            }
        }
    }
}