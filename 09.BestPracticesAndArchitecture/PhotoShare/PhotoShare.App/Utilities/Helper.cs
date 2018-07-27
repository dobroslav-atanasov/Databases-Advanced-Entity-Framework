namespace PhotoShare.App.Utilities
{
    using System.Linq;
    using Data;

    public class Helper
    {
        public static bool IsUsernameExist(PhotoShareContext dbContext, string username)
        {
            bool isUsernameExist = dbContext
                    .Users
                    .Any(u => u.Username == username);

            return isUsernameExist;
        }

        public static bool IsTownNameExist(PhotoShareContext dbContext, string townName)
        {
            bool isTownNameExist = dbContext
                    .Towns
                    .Any(t => t.Name == townName);

            return isTownNameExist;
        }

        public static bool IsValidProperty(string property)
        {
            string[] allowedProperties = new string[] { "Password", "BornTown", "CurrentTown" };

            if (allowedProperties.Contains(property))
            {
                return true;
            }

            return false;
        }

        public static bool IsAlbumNameExist(PhotoShareContext dbContext, string albumName)
        {
            bool isAlbumNameExist = dbContext
                .Albums
                .Any(a => a.Name == albumName);

            return isAlbumNameExist;
        }

        public static bool IsValidTags(PhotoShareContext dbContext, string[] tags)
        {
            if (tags.Length == 0)
            {
                return false;
            }

            foreach (string tag in tags)
            {
                string tagName = $"#{tag}";
                if (dbContext.Tags.Count() == 0)
                {
                    return false;
                }

                if (!dbContext.Tags.Any(t => t.Name == tagName))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsAlbumTagExist(PhotoShareContext dbContext, string albumName, string tagName)
        {
            bool isAlbumTagExist = dbContext
                .AlbumTags
                .Any(at => at.Album.Name == albumName && at.Tag.Name == tagName);

            return isAlbumTagExist;
        }

        public static bool IsFriendshipExist(PhotoShareContext dbContext, string firstUsername, string secondUsername)
        {
            bool isFriendshipExist = dbContext
                .Friendships
                .Any(f => f.User.Username == firstUsername && f.Friend.Username == secondUsername);

            return isFriendshipExist;
        }
    }
}