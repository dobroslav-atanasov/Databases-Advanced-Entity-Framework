namespace PhotoShare.App.Core
{
    using System;
    using Models;
    using Utilities;

    public class Authentication
    {
        public static User CurrentUser { get; set; }

        public static void Authorize()
        {
            if (CurrentUser == null)
            {
                throw new InvalidOperationException(Constants.NotLogin);
            }
        }

        public static User GetCurrentUser()
        {
            return CurrentUser;
        }
    }
}