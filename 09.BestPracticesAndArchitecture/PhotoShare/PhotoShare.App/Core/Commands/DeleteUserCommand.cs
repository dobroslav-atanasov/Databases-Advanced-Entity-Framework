namespace PhotoShare.App.Core.Commands
{
    using System;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class DeleteUserCommand : ICommand
    {
        // DeleteUser
        public string Execute(string[] arguments)
        {
            Check.CheckLength(0, arguments);
            Authentication.Authorize();
            PhotoShareContext dbContext = new PhotoShareContext();

            User currentUser = Authentication.CurrentUser;

            if (currentUser.IsDeleted == false)
            {
                throw new InvalidOperationException(string.Format(Constants.UserAlreadyDeleted, currentUser.Username));
            }

            using (dbContext)
            {
                currentUser.IsDeleted = true;
                dbContext.SaveChanges();

                return string.Format(Constants.DeleteUser, currentUser.Username);
            }
        }
    }
}