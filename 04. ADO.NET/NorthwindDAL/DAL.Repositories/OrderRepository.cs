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
                            var order = new Order();

                            order.Set(reader);
                            order.GetStatus();

                            resultOrders.Add(order);
                        }
                    }
                }
            }

            return resultOrders;
        }

        public Order GetOrderDetailByOrderID(int id)
        {
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

        public int Add(Order order)
        {
            int newID;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
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

        public int Update(Order order)
        {
            int affected = 0;

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

            var status = GetOrderDetailByOrderID(orderID).Status;
            if (status == OrderStatus.New || status == OrderStatus.InProgress)
            {
                using (var connection = ProviderFactory.CreateConnection())
                {
                    var order = new Order();

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
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SetInProgress";

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

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
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
