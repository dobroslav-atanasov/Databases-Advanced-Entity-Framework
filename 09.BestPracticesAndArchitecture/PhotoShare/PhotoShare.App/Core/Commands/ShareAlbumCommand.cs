namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;
    using Utilities;

    public class ShareAlbumCommand : ICommand
    {
        // ShareAlbum <albumId> <permission>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            Authentication.Authorize();
            int albumId = int.Parse(arguments[0]);
            string role = arguments[1];
            User currentUser = Authentication.GetCurrentUser();
            PhotoShareContext dbContext = new PhotoShareContext();
            
            Album album = dbContext.Albums.Find(albumId);

            if (album == null)
            {
                throw new ArgumentException(string.Format(Constants.AlbumNotFound, albumId));
            }

            Role permission;
            bool isValidPermission = Enum.TryParse(role, out permission);
            if (!isValidPermission)
            {
                throw new ArgumentException(Constants.NotValidPermision);
            }

            if (permission == Role.Viewer)
            {
                throw new ArgumentException(Constants.ViewerPermission);
            }

            AlbumRole albumRole = dbContext
                .AlbumRoles
                .FirstOrDefault(ar => ar.AlbumId == album.Id && ar.UserId == currentUser.Id);

            if (albumRole == null)
            {
                throw new ArgumentException(Constants.NotPermission);
            }

            using (dbContext)
            {
                album.IsPublic = true;
                dbContext.SaveChanges();
            }

            return string.Format(Constants.ShareAlbum, currentUser.Username, album.Name, permission);
        }
    }
}