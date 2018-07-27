namespace PhotoShare.App.Core.Contracts
{
    public interface ICommandDispatcher
    {
        string DispatchCommand(string[] inputParts);
    }
}