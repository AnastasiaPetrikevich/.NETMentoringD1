using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Ninject;

namespace DependencyResolver
{
    public static class Resolver
    {
        static Resolver()
        {
            Kernel = new StandardKernel();
            Kernel.ConfigurateResolver();
        }

		public const string dbConnection = @"Data Source=EPBYMINW8361;Initial Catalog=Northwind;Persist Security Info=False;User ID=sa;Password=Elite123;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

		public static IKernel Kernel { get; private set; }

        public static void ConfigurateResolver(this IKernel kernel)
        {
            kernel.Bind<IOrderRepository>().To<OrderRepository>().InSingletonScope()
                                           .WithConstructorArgument("connectionString", dbConnection)
                                           .WithConstructorArgument("provider", "System.Data.SqlClient");

            kernel.Bind<ICustomerRepository>().To<CustomerRepository>().InSingletonScope()
                                          .WithConstructorArgument("connectionString", dbConnection)
                                          .WithConstructorArgument("provider", "System.Data.SqlClient");
        }
    }
}