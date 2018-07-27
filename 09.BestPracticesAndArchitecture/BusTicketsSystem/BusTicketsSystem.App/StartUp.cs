namespace BusTicketsSystem.App
{
    using System;
    using Core;
    using Core.Contracts;
    using Core.IO;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Contracts;

    public class StartUp
    {
        public static void Main()
        {
            IServiceProvider serviceProvider = ConfigureServices();

            IEngine engine = new Engine(serviceProvider);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<BusTicketsSystemContext>(opt => opt.UseSqlServer(Configuration.ConnectionString).UseLazyLoadingProxies());

            serviceCollection.AddTransient<IReader, Reader>();
            serviceCollection.AddTransient<IWriter, Writer>();
            serviceCollection.AddTransient<ICommandDispatcher, CommandDispatcher>();
            serviceCollection.AddTransient<IBusCompanyService, BusCompanyService>();
            serviceCollection.AddTransient<ITownService, TownService>();
            serviceCollection.AddTransient<IBusStationService, BusStationService>();
            serviceCollection.AddTransient<ICustomerService, CustomerService>();
            serviceCollection.AddTransient<ITripService, TripService>();
            serviceCollection.AddTransient<IBankAccountService, BankAccountService>();
            serviceCollection.AddTransient<ITicketService, TicketService>();
            serviceCollection.AddTransient<IReviewService, ReviewService>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}