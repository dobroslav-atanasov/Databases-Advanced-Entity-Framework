namespace PhotoShare.App.Core.Contracts
{
    public interface IWriter
    {
        void Write(string text);

        void WriteLine(string text);
    }
}