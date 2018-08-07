namespace TeamBuilder.App.Core
{
    using System;
    using System.Linq;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Contracts;

    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            IReader reader = this.serviceProvider.GetService<IReader>();
            IWriter writer = this.serviceProvider.GetService<IWriter>();
            IDatabaseService databaseService = this.serviceProvider.GetService<IDatabaseService>();
            databaseService.InitializeDatabase();

            while (true)
            {
                string input = reader.ReadLine();
                string[] inputParts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string commandName = inputParts[0];
                string[] arguments = inputParts.Skip(1).ToArray();

                try
                {
                    ICommand command = CommandParser.ParseCommand(this.serviceProvider, commandName);
                    string result = command.Execute(arguments);
                    writer.WriteLine(result);
                }
                catch (Exception e)
                {
                    writer.WriteLine(e.GetBaseException().Message);
                }
            }
        }
    }
}