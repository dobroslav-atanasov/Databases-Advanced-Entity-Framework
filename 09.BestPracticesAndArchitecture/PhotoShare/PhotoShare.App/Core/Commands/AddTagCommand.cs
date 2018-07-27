namespace PhotoShare.App.Core.Commands
{
    using Contracts;
    using Models;
    using Data;
    using Utilities;

    public class AddTagCommand : ICommand
    {
        // AddTag <tag>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);
            Authentication.Authorize();
            string tagName = arguments[0].ValidateOrTransform();
            PhotoShareContext dbContext = new PhotoShareContext();

            using (dbContext)
            {
                Tag tag = new Tag()
                {
                    Name = tagName
                };

                dbContext.Tags.Add(tag);
                dbContext.SaveChanges();
            }

            return string.Format(Constants.AddTag, tagName);
        }
    }
}