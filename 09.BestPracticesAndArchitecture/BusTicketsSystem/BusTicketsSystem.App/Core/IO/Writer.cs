namespace BusTicketsSystem.App.Core.IO
{
    using System;
    using Contracts;

    public class Writer : IWriter
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}