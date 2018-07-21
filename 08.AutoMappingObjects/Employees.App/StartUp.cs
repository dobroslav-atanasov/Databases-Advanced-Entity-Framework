namespace Employees.App
{
    using System;
    using AutoMapper;
    using Core;
    using Core.IO;
    using Core.IO.Contracts;
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

            Engine engine = new Engine(serviceProvider);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeeContext>(o => o.UseSqlServer(Configuration.ConnectionString));
            serviceCollection.AddTransient<IDatabaseService, DatabaseService>();
            serviceCollection.AddTransient<IEmployeeService, EmployeeService>();
            serviceCollection.AddTransient<IManagerService, ManagerService>();
            serviceCollection.AddTransient<IReader, Reader>();
            serviceCollection.AddTransient<IWriter, Writer>();
            serviceCollection.AddAutoMapper(m => m.AddProfile<AutomapperProfile>());

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}