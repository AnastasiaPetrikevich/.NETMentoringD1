using DAL.Entities;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbProviderFactory ProviderFactory;
        private readonly string ConnectionString;

        public OrderRepository(string connectionString, string provider)
        {
            ProviderFactory = DbProviderFactories.GetFactory(provider);
            ConnectionString = connectionString;
        }

        //NOTE: not for this task of course but for large applications pagination/filter is added for GetAll method to limit the number of retrieved documents
        //also there are no try-catch sections so your exceptions will be thrown to BLL
        //when I googled it I've met both opinions: that it's ok that BLL catches exceptions and that it's a wrong approach
        //Personally I would prefer to catch SQL exceptions and throw my own exceptions
        public IEnumerable<Order> GetAll() 
        {
            var resultOrders = new List<Order>();

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Orders";
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //var order = new Order();

                            //order.Set(reader);
                            //order.GetStatus();

                            //resultOrders.Add(order);'

                            //need to replace with one method, that takes a reader as an Argument and returns initialized Order
                        }
                    }
                }
            }

            return resultOrders;
        }

        public Order GetOrderDetailByOrderID(int id)
        {
            //1. Too long method. Please separate it into smaller ones. And replace Set-GetStatus with one method as I mentioned above
            //2. Please rename the method to GetDetailedOrder or smth like that. We 
            //3.For the task we need only product name so I don't think we need to retrieve full Product data from DB by default
            //I would create another ProductRepository that works with product.
            //And let a BLL decide what they want to retrieve
            //Or add a parameter to a function like "includeProduct" and get Product if the parameter set to True
            //4. I'm not sure but probably it's possible to use inner join for OrderDetail and product
            //So instead of:
            //"SELECT * FROM dbo.[Order Details] WHERE OrderID = @id"; + "SELECT * FROM dbo.Products WHERE ProductID = @id";
            //there will be join that will give you all results without additional foreach
            //if you have time please look at that
            using (var connection = ProviderFactory.CreateConnection())
            {
                Order order = null;

                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT * FROM dbo.Orders WHERE OrderID = @id; " +
                        "SELECT * FROM dbo.[Order Details] WHERE OrderID = @id";
                    command.CommandType = CommandType.Text;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;

                    command.Parameters.Add(paramId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            order = new Order();

                            reader.Read();
                            order.Set(reader);
                            order.GetStatus();

                            reader.NextResult();
                            var details = new List<OrderDetail>();

                            while (reader.Read())
                            {
                                var detail = new OrderDetail();
                                detail.Set(reader);
                                detail.Order = order;

                                details.Add(detail);
                            }

                            order.Details = details;
                        }
                    }
                }

                if (order != null)
                {
                    foreach (var detail in order.Details)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText =
                                "SELECT * FROM dbo.Products WHERE ProductID = @id";
                            command.CommandType = CommandType.Text;

                            var productId = command.CreateParameter();
                            productId.ParameterName = "@id";
                            productId.Value = detail.ProductID;

                            command.Parameters.Add(productId);

                            using (var productReader = command.ExecuteReader())
                            {
                                productReader.Read();
                                detail.Product = new Product();
                                detail.Product.Set(productReader);
                            }
                        }
                    }
                }

                return order;
            }
        }

        //Too long method. Please separate it into smaller ones. And replace Set-GetStatus with one method as I mentioned above
        public int Add(Order order)
        {
            int newID;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    //why not using reflection to get Order field names?
                    //but in general I think hardcoding insert into statement would be easier to read and maintain
                    //you just need a placeholder to fill it with values or passing Nulls
                    var orderFields = new string[] { /*"OrderID",*/ "CustomerID", "EmployeeID", "OrderDate",
                                                     "RequiredDate", "ShippedDate", "ShipVia", "Freight",
                                                     "ShipName", "ShipAddress", "ShipCity", "ShipRegion",
                                                     "ShipPostalCode", "ShipCountry"
                                                   };
                    Dictionary<string, object> orderFieldsValues = new Dictionary<string, object>();
                    StringBuilder commandText = new StringBuilder();

                    object value;
                    commandText.Append("INSERT INTO Orders (");
                    for (var i = 0; i < orderFields.Length - 1; i++)
                    {
                        value = order.GetType().GetProperty(orderFields[i]).GetValue(order);
                        if (value != null)
                        {
                            commandText.Append(orderFields[i]);
                            commandText.Append(", ");
                            orderFieldsValues.Add(orderFields[i], value);
                        }
                    }
                    value = order.GetType().GetProperty(orderFields[orderFields.Length - 1]).GetValue(order);
                    if (value != null)
                    {
                        commandText.Append(orderFields[orderFields.Length - 1]);
                        orderFieldsValues.Add(orderFields[orderFields.Length - 1], value);
                    }
                    else
                    {
                        commandText.Remove(commandText.Length - 2, 2);
                    }

                    // insert values into command
                    commandText.Append(") VALUES (");
                    foreach (var field in orderFieldsValues.Keys)
                    {
                        commandText.Append("@");
                        commandText.Append(field);
                        commandText.Append(", ");
                    }
                    commandText.Remove(commandText.Length - 2, 2);
                    commandText.Append("); SELECT OrderID FROM  Orders WHERE OrderID = SCOPE_IDENTITY()");

                    command.CommandText = commandText.ToString();
                    command.CommandType = CommandType.Text;

                    DbParameter paramId;

                    foreach (var field in orderFieldsValues.Keys)
                    {
                        paramId = command.CreateParameter();
                        paramId.ParameterName = "@" + field;
                        paramId.Value = order.GetType().GetProperty(field).GetValue(order);
                        command.Parameters.Add(paramId);
                    }

                    newID = (int)command.ExecuteScalar();
                }

                return newID;
            }
        }

        //the same as for insert:
        //you can have complete update-query with a placeholder that should be replaced with null or values
        public int Update(Order order)
        {
            int affected = 0;
            //you retrieve order with all the details just to get the status
            //it's better to have separate GetOrder method that will simply return an Order, without details
            var status = GetOrderDetailByOrderID(order.OrderID).Status;
            if (status == OrderStatus.New)
            {

                using (var connection = ProviderFactory.CreateConnection())
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        var orderFields = new string[] { "CustomerID", "EmployeeID", "RequiredDate",
                                                     "ShipVia", "Freight", "ShipName",
                                                     "ShipAddress", "ShipCity", "ShipRegion",
                                                     "ShipPostalCode", "ShipCountry"
                                                   };
                        Dictionary<string, object> orderFieldsValues = new Dictionary<string, object>();
                        StringBuilder commandText = new StringBuilder();

                        object value;
                        commandText.Append("UPDATE Orders set ");
                        for (var i = 0; i < orderFields.Length - 1; i++)
                        {
                            value = order.GetType().GetProperty(orderFields[i]).GetValue(order);
                            if (value != null)
                            {
                                commandText.Append(orderFields[i]);
                                commandText.Append(" = ");
                                commandText.Append("@" + orderFields[i]);
                                commandText.Append(", ");
                                orderFieldsValues.Add(orderFields[i], value);
                            }
                        }
                        commandText.Remove(commandText.Length - 2, 2);
                        commandText.Append(" WHERE OrderID = @Id");

                        command.CommandText = commandText.ToString();
                        command.CommandType = CommandType.Text;

                        DbParameter paramId;
                        paramId = command.CreateParameter();
                        paramId.ParameterName = "@Id";
                        paramId.Value = order.OrderID;
                        command.Parameters.Add(paramId);

                        foreach (var field in orderFieldsValues.Keys)
                        {
                            paramId = command.CreateParameter();
                            paramId.ParameterName = "@" + field;
                            paramId.Value = order.GetType().GetProperty(field).GetValue(order);
                            command.Parameters.Add(paramId);
                        }

                        affected = command.ExecuteNonQuery();
                    }
                }
            }

            return affected;
        }

        public int Delete(int orderID)
        {
            int affected = 0;

            var status = GetOrderDetailByOrderID(orderID).Status;//same as above
            if (status == OrderStatus.New || status == OrderStatus.InProgress)
            {
                using (var connection = ProviderFactory.CreateConnection())
                {
                    var order = new Order();//not used

                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "delete FROM Orders WHERE OrderID = @id";
                        command.CommandType = CommandType.Text;

                        var paramId = command.CreateParameter();
                        paramId.ParameterName = "@id";
                        paramId.Value = orderID;
                        command.Parameters.Add(paramId);

                        affected = command.ExecuteNonQuery();
                    }
                }
            }

            return affected;
        }

        public int SetInProgressStatus(int orderID)
        {
            var affected = 0;
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure; //I don't really understand why you decided to use a SP for the task :)
                                                                        //According to the task I suppose there should be methods that update table
                                                                        //Because SPs mentioned only for Task #7
                    command.CommandText = "SetInProgress";

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@OrderID";
                    paramId.Value = orderID;
                    command.Parameters.Add(paramId);

                    //if order is updated paramId.Value will be equal to orderID. if it's not updated paramId.Value will be epmty object
                    paramId = command.CreateParameter(); 
                    paramId.ParameterName = "@id";
                    paramId.Value = orderID; //I don't understand why but it's needed to set some value
                                            // I don't know why too :)
                    paramId.Direction = ParameterDirection.Output;
                    command.Parameters.Add(paramId);

                    affected = command.ExecuteNonQuery();
                }
            }

            return affected;
        }

        //the same. I suppose it should not use SPs
        public int SetDoneStatus(int orderID)
        {
            var affected = 0;
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SetDone";

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@OrderID";
                    paramId.Value = orderID;
                    command.Parameters.Add(paramId);

                    //if order is updated paramId.Value will be equal to orderID. if it's not updated paramId.Value will be epmty object
                    paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = orderID; //I don't understand why but it's needed to set some value
                    paramId.Direction = ParameterDirection.Output;
                    command.Parameters.Add(paramId);

                    affected = command.ExecuteNonQuery();
                }
            }

            return affected;
        }

        public IEnumerable<CustOrdersDetail> GetCustOrdersDetails(int orderID)
        {
            List<CustOrdersDetail> productQuantities = new List<CustOrdersDetail>();

            //using (var connection = ProviderFactory.CreateConnection())
            //{
            //    connection.ConnectionString = ConnectionString;
            //    connection.Open();

            //    using (var command = connection.CreateCommand())

            //These lines of code are used across all the methods. 
            //Please check if it can be moved to a single method
            //And method-specific logic can be moved to its own private methods
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CustOrdersDetail";

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@OrderID";
                    paramId.Value = orderID;
                    command.Parameters.Add(paramId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var productQuantity = new CustOrdersDetail();

                            productQuantity.Set(reader);

                            productQuantities.Add(productQuantity);
                        }
                    }
                }
            }

            return productQuantities;
        }
    }
}
