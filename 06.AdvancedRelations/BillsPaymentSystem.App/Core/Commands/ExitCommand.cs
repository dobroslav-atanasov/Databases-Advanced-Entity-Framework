namespace BillsPaymentSystem.App.Core.Commands
{
    using System;
    using Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] arguments)
        {
            Environment.Exit(0);
            return string.Empty;
        }
    }
}