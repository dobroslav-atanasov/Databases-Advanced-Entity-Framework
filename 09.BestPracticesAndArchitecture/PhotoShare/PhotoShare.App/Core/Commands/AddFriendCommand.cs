namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class AddFriendCommand : ICommand
    {
        // AddFriend <friendUsername> 
        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);
            Authentication.Authorize();
            string friendUsername = arguments[0];
            User user = Authentication.GetCurrentUser();

            PhotoShareContext dbContext = new PhotoShareContext();

            User friend = dbContext
                .Users
                .FirstOrDefault(u => u.Username == friendUsername);

            if (friend == null)
            {
                throw new ArgumentException(string.Format(Constants.UserNotFound, friendUsername));
            }

            if (Helper.IsFriendshipExist(dbContext, user.Username, friendUsername))
            {
                throw new InvalidOperationException(string.Format(Constants.AlreadyFriends, friendUsername, user.Username));
            }

            using (dbContext)
            {
                Friendship friendship = new Friendship()
                {
                    UserId = user.Id,
                    FriendId = friend.Id
                };

                dbContext.Friendships.Add(friendship);
                dbContext.SaveChanges();
            }

            return string.Format(Constants.AddFriend, friendUsername, user.Username);
        }
    }
}