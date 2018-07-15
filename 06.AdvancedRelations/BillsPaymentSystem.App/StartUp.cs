namespace BillsPaymentSystem.App
{
    using System;
    using Core;
    using Core.IO;
    using Core.IO.Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SeedData;
    using Services;
    using Services.Contracts;

    public class StartUp
    {
        public static void Main()
        {
            IServiceProvider serviceProvider = ConfigureServices();

            Engine engine = new Engine(serviceProvider);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<BillsPaymentSystemContext>(o => o.UseSqlServer(Configuration.ConnectionString));
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IReader, Reader>();
            serviceCollection.AddTransient<IWriter, Writer>();
            serviceCollection.AddTransient<IBankAccountService, BankAccountService>();
            serviceCollection.AddTransient<ICreditCardService, CreditCardService>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}