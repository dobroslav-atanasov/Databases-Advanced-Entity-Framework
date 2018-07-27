namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class AddTagToCommand : ICommand
    {
        // AddTagTo <album> <tag>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            Authentication.Authorize();;
            string albumName = arguments[0];
            string tagName = arguments[1];
            User currentUser = Authentication.GetCurrentUser();
            PhotoShareContext dbContext = new PhotoShareContext();

            Album album = dbContext
                .Albums
                .FirstOrDefault(a => a.Name == albumName);

            Tag tag = dbContext
                .Tags
                .FirstOrDefault(t => t.Name == $"#{tagName}");

            if (album == null || tag == null)
            {
                throw new ArgumentException(Constants.AlbumOrTagDoNotExist);
            }

            if (Helper.IsAlbumTagExist(dbContext, albumName, tagName))
            {
                throw new ArgumentException(Constants.AlbumOrTagDoNotExist);
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
                AlbumTag albumTag = new AlbumTag()
                {
                    AlbumId = album.Id,
                    TagId = tag.Id
                };

                dbContext.AlbumTags.Add(albumTag);
                dbContext.SaveChanges();
            }

            return string.Format(Constants.AddTagToAlbum, tagName, albumName);
        }
    }
}