namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class PrintFriendsListCommand : ICommand
    {
        // PrintFriendsList <username>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);
            string username = arguments[0];
            PhotoShareContext dbContext = new PhotoShareContext();

            User user = dbContext
                .Users
                .FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new ArgumentException(string.Format(Constants.UserNotFound, username));
            }

            Friendship[] friendships = dbContext
                .Friendships
                .Where(u => u.UserId == user.Id)
                .ToArray();

            if (friendships.Length == 0)
            {
                return Constants.UserDoesNotHaveFriends;
            }

            using (dbContext)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Friends:");
                foreach (Friendship friend in friendships)
                {
                    sb.AppendLine($" - {friend.Friend.Username}");
                }

                return sb.ToString().Trim();
            }
        }
    }
}