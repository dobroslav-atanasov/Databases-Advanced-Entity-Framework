namespace PhotoShare.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Contracts;
    using Utilities;

    public class CommandDispatcher : ICommandDispatcher
    {
        private string Suffix = "Command";

        public string DispatchCommand(string[] inputParts)
        {
            string commandName = $"{inputParts[0]}{this.Suffix}";
            string[] arguments = inputParts.Skip(1).ToArray();

            Type type = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == commandName);

            if (type == null)
            {
                throw new InvalidOperationException(string.Format(Constants.InvalidCommand, inputParts[0]));
            }
            
            ICommand command = (ICommand) Activator.CreateInstance(type);
            string result = command.Execute(arguments);
            return result;
        }
    }
}