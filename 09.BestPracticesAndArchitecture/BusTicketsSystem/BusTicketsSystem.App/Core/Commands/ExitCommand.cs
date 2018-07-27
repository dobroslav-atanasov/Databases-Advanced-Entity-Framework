namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Utilities;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] arguments)
        {
            Check.CheckLength(0, arguments);
            Environment.Exit(0);
            return string.Empty;
        }
    }
}