namespace BillsPaymentSystem.App.Core
{
    using System;
    using System.Linq;
    using Commands.Contracts;
    using IO.Contracts;
    using Microsoft.Extensions.DependencyInjection;

    public class Engine
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

            while (true)
            {
                writer.Write("Enter command: ");
                string input = reader.ReadLine();

                string[] inputParts = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                string commandName = inputParts[0];
                string[] arguments = inputParts.Skip(1).ToArray();

                try
                {
                    ICommand command = CommandParser.ParseCommand(this.serviceProvider, commandName);
                    string commandResult = command.Execute(arguments);

                    writer.WriteLine(commandResult);
                }
                catch (ArgumentException ae)
                {
                    writer.WriteLine(ae.Message);
                }
                catch (InvalidOperationException ioe)
                {
                    writer.WriteLine(ioe.Message);
                }
                catch (Exception e)
                {
                    writer.WriteLine(e.InnerException.Message);
                }
            }
        }
    }
}