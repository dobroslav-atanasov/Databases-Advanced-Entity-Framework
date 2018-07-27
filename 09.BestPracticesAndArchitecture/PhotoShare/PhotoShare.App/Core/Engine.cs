namespace PhotoShare.App.Core
{
    using System;
    using Contracts;

    public class Engine : IEngine
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IReader reader;
        private readonly IWriter writer;

        public Engine(ICommandDispatcher commandDispatcher, IReader reader, IWriter writer)
        {
            this.commandDispatcher = commandDispatcher;
            this.reader = reader;
            this.writer = writer;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string input = this.reader.ReadLine();
                    string[] inputParts = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                    string result = this.commandDispatcher.DispatchCommand(inputParts);
                    this.writer.WriteLine(result);
                }
                catch (Exception e)
                {
                    this.writer.WriteLine(e.Message);
                }
            }
        }
    }
}
