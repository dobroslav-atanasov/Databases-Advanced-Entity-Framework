namespace BusTicketsSystem.App.Core.Contracts
{
    using System;

    public interface ICommandDispatcher
    {
        string ParseCommand(IServiceProvider serviceProvider, string[] inputParts);
    }
}