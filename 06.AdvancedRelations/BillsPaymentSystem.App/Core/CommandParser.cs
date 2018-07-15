namespace BillsPaymentSystem.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Commands.Contracts;

    public static class CommandParser
    {
        public static ICommand ParseCommand(IServiceProvider serviceProvider, string commandName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            Type commandType = types.SingleOrDefault(t => t.Name == $"{commandName}Command");

            if (commandType == null)
            {
                throw new InvalidOperationException("Invalid command!");
            }

            ConstructorInfo constructor = commandType.GetConstructors().First();
            Type[] constructorParameters = constructor
                .GetParameters()
                .Select(cp => cp.ParameterType)
                .ToArray();

            object[] services = constructorParameters
                .Select(serviceProvider.GetService)
                .ToArray();

            ICommand command = (ICommand)constructor.Invoke(services);
            return command;
        }
    }
}