namespace PhotoShare.App.Utilities
{
    public static class Constants
    {
        // Command constants
        public static string InvalidCommand = "Command {0} not invalid!";
        public static string InvalidArgumentsCount = "Arguments count are invalid!";

        // User constants
        public static string PasswordsDoNotMatch = "Passwords do not match!";
        public static string UsernameIsTaken = "Username {0} is already taken!";
        public static string RegisterUser = "User {0} was registered successfully!";
        public static string UserNotFound = "User {0} not found!";
        public static string ModifyUser = "User {0} {1} is {2}!";
        public static string DeleteUser = "User {0} was deleted from the database!";
        public static string UserAlreadyDeleted = "User {0} is already deleted!";
        public static string UserDoesNotFound = "User with {0} was not found!";
        public static string UserDoesNotHaveFriends = "No friends for this user. :(";

        // Town constans
        public static string AddTown = "Town {0} was added successfully!";
        public static string TownAlreadtExist = "Town {0} was already added!";
        public static string TownDoesNotExist = "Town {0} not found!";

        // Property constants
        public static string PropertyNotFount = "Property {0} not supported!";
        public static string InvalidPassword = "Invalid Password!";

        // Tag constants
        public static string AddTag = "{0} was added successfully to database!";
        public static string InvalidTags = "Invalid tags!";

        // Album constats
        public static string AlbumExists = "Album {0} exists!";
        public static string CreateAlbum = "Album {0} successfully created!";
        public static string ColorNotFound = "Color {0} not found!";
        public static string NotValidPermision = "Permission must be either “Owner” or “Viewer”!";
        public static string ShareAlbum = "Username {0} added to album {1} ({2})";
        public static string AlbumNotFound = "Album {0} not found!";
        public static string ViewerPermission = "Permission must be “Owner” to share!";
        public static string NotPermission = "You do not have permission to share!";
        public static string NotUploadPicture = "You do not have permission to upload!";

        // AlbumTag constats
        public static string AddTagToAlbum = "Tag {0} added to {1}!";
        public static string AlbumOrTagDoNotExist = "Either tag or album do not exist!";

        // Friendship constants
        public static string AddFriend = "Friend {0} added to {1}!";
        //public static string UsersNotExist = "{0} not found!";
        public static string AlreadyFriends = "{0} is already a friend to {1}!";
        public static string ThereIsNoRequest = "{0} has not added {1} as a friend!";
        public static string AcceptFriend = "{0} accepted {1} as a friend!";

        // Picture constants
        public static string UploadPicture = "Picture {0} added to {1}!";

        // Login and Logout constants
        public static string Login = "User {0} successfully logged in!";
        public static string InvalidUsernameOrPassword = "Invalid username or password!";
        public static string AlreadyLogged = "You should logout first!";
        public static string Logout = "User {0} successfully logged out!";
        public static string NotLogin = "You should log in first in order to logout!";
    }
}