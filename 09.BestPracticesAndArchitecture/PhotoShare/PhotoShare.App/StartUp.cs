namespace PhotoShare.App
{
    using Core;
    using Core.Contracts;
    using Core.IO;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            ResetDatabase();

            ICommandDispatcher commandDispatcher = new CommandDispatcher();
            IReader reader = new Reader();
            IWriter writer = new Writer();
            IEngine engine = new Engine(commandDispatcher, reader, writer);
            engine.Run();
        }

        private static void ResetDatabase()
        {
            PhotoShareContext dbContext = new PhotoShareContext();

            using (dbContext)
            {
                //dbContext.Database.EnsureDeleted();
                //dbContext.Database.EnsureCreated();

                dbContext.Database.Migrate();
            }
        }
    }
}