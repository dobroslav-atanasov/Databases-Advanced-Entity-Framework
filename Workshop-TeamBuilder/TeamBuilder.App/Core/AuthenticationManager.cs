namespace TeamBuilder.App.Core
{
    using System;
    using Models;
    using Services.Utilities;

    public static class AuthenticationManager
    {
        private static User currentUser;

        public static void Login(User user)
        {
            if (currentUser != null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }
            currentUser = user;
        }

        public static User Logout()
        {
            Authorize();
            User user = currentUser;
            currentUser = null;

            return user;
        }

        public static void Authorize()
        {
            if (currentUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }
        }

        public static bool IsAuthenticated()
        {
            if (currentUser != null)
            {
                return true;
            }

            return false;
        }

        public static User GetCurrentUser()
        {
            return currentUser;
        }
    }
}