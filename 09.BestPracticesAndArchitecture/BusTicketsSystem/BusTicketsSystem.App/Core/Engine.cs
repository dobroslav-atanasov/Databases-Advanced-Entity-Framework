namespace BusTicketsSystem.App.Core
{
    using System;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

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
            ICommandDispatcher commandDispatcher = this.serviceProvider.GetService<ICommandDispatcher>();

            while (true)
            {
                string input = reader.ReadLine();
                string[] inputParts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    string result = commandDispatcher.ParseCommand(this.serviceProvider, inputParts);
                    writer.WriteLine(result);
                }
                catch (Exception e)
                {
                    writer.WriteLine(e.Message);
                }
            }
        }
    }
}