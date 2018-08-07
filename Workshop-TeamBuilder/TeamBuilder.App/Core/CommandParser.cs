namespace TeamBuilder.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Contracts;
    using Services.Utilities;

    public class CommandParser
    {
        public static ICommand ParseCommand(IServiceProvider serviceProvider, string commandName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            Type type = types
                .FirstOrDefault(t => t.Name == $"{commandName}Command");

            if (type == null)
            {
                throw new NotSupportedException(string.Format(Constants.ErrorMessages.InvalidCommandName, commandName));
            }

            ConstructorInfo constructor = type.GetConstructors().First();
            Type[] constructorParameters = constructor
                .GetParameters()
                .Select(cp => cp.ParameterType)
                .ToArray();

            object[] services = constructorParameters
                .Select(serviceProvider.GetService)
                .ToArray();

            ICommand command = (ICommand) constructor.Invoke(services);
            return command;
        }
    }
}