using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DAL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbProviderFactory ProviderFactory;
        private readonly string ConnectionString;

        public CustomerRepository(string connectionString, string provider)
        {
            ProviderFactory = DbProviderFactories.GetFactory(provider);
            ConnectionString = connectionString;
        }

        public IEnumerable<string> GetIDs()
        {
            var resultIDs = new List<string>();

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT CustomerID FROM dbo.Customers";
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultIDs.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return resultIDs;
        }

        public IEnumerable<ProductQuantity> GetProductQuantities(string customerID)
        {
            List<ProductQuantity> productQuantities = new List<ProductQuantity>();

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CustOrderHist";

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@CustomerID";
                    paramId.Value = customerID;
                    command.Parameters.Add(paramId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var productQuantity = new ProductQuantity();

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
