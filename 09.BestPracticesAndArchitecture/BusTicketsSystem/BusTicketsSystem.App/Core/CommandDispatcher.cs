namespace BusTicketsSystem.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Contracts;
    using Utilities;

    public class CommandDispatcher : ICommandDispatcher
    {
        public string ParseCommand(IServiceProvider serviceProvider, string[] inputParts)
        {
            string commandName = inputParts[0];
            string[] arguments = inputParts.Skip(1).ToArray();

            Type type = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == $"{commandName}{AppConstants.Suffix}");

            if (type == null)
            {
                throw new InvalidOperationException(string.Format(AppConstants.InvalidCommandName, commandName));
            }

            ConstructorInfo constructor = type.GetConstructors().First();
            Type[] constructorParameters = constructor
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();

            object[] services = constructorParameters
                .Select(serviceProvider.GetService)
                .ToArray();

            ICommand command = (ICommand) constructor.Invoke(services);
            string result = command.Execute(arguments);
            return result;
        }
    }
}