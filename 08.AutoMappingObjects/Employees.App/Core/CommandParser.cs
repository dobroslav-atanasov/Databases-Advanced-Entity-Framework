namespace Employees.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Commands.Contracts;

    public class CommandParser
    {
        public static ICommand ParseCommand(IServiceProvider serviceProvider, string commandName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            Type type = types.SingleOrDefault(t => t.Name == $"{commandName}Command");

            if (type == null)
            {
                throw new InvalidOperationException("Invalid command!");
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
            return command;
        }
    }
}