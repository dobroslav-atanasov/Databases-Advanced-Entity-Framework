namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class AcceptFriendCommand : ICommand
    {
        // AcceptFriend <username1> <username2>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);

            string firstUsername = arguments[0];
            string secondUsername = arguments[1];

            PhotoShareContext dbContext = new PhotoShareContext();

            User user = dbContext
                .Users
                .FirstOrDefault(u => u.Username == firstUsername);

            if (user == null)
            {
                throw new ArgumentException(string.Format(Constants.UserNotFound, firstUsername));
            }

            User friend = dbContext
                .Users
                .FirstOrDefault(u => u.Username == secondUsername);

            if (friend == null)
            {
                throw new ArgumentException(string.Format(Constants.UserNotFound, secondUsername));
            }

            if (Helper.IsFriendshipExist(dbContext, firstUsername, secondUsername))
            {
                throw new InvalidOperationException(string.Format(Constants.AlreadyFriends, secondUsername, firstUsername));
            }

            if (Helper.IsFriendshipExist(dbContext, firstUsername, secondUsername) && Helper.IsFriendshipExist(dbContext, secondUsername, firstUsername))
            {
                throw new InvalidOperationException(string.Format(Constants.ThereIsNoRequest, secondUsername, firstUsername));
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

            return string.Format(Constants.AcceptFriend, firstUsername, secondUsername);
        }
    }
}