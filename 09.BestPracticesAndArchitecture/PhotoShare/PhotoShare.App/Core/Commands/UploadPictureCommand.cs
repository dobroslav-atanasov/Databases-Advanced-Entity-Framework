namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class UploadPictureCommand : ICommand
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(3, arguments);
            Authentication.Authorize();
            string albumName = arguments[0];
            string pictureTitle = arguments[1];
            string pictureFilePath = arguments[2];
            User currentUser = Authentication.GetCurrentUser();

            PhotoShareContext dbContext = new PhotoShareContext();

            Album album = dbContext
                .Albums
                .FirstOrDefault(a => a.Name == albumName);

            if (album == null)
            {
                throw new ArgumentException(string.Format(Constants.AlbumNotFound, albumName));
            }

            AlbumRole albumRole = dbContext
                .AlbumRoles
                .FirstOrDefault(ar => ar.AlbumId == album.Id && ar.UserId == currentUser.Id);

            if (albumRole == null)
            {
                throw new ArgumentException(Constants.NotUploadPicture);
            }

            using (dbContext)
            {
                Picture picture = new Picture()
                {
                    AlbumId = album.Id,
                    Title = pictureTitle,
                    Path = pictureFilePath
                };

            }

            return string.Format(Constants.UploadPicture, pictureTitle, albumName);
        }
    }
}