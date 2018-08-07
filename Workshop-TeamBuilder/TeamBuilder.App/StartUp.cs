namespace TeamBuilder.App
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

            serviceCollection.AddDbContext<TeamBuilderContext>(o => o.UseSqlServer(Configuration.ConnectionString));
            serviceCollection.AddTransient<IReader, Reader>();
            serviceCollection.AddTransient<IWriter, Writer>();
            serviceCollection.AddTransient<IDatabaseService, DatabaseService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IEventService, EventService>();
            serviceCollection.AddTransient<ITeamService, TeamService>();
            serviceCollection.AddTransient<IUserTeamService, UserTeamService>();
            serviceCollection.AddTransient<IInvitationService, InvitationService>();
            serviceCollection.AddTransient<IEventTeamService, EventTeamService>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}