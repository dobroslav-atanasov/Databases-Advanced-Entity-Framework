namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;
    using Utilities;

    public class CreateAlbumCommand : ICommand
    {
        // CreateAlbum <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(string[] arguments)
        {
            Authentication.Authorize();
            string albumTitle = arguments[0];
            string bgColor = arguments[1];
            string[] tagNames = arguments.Skip(2).ToArray();
            User currentUser = Authentication.GetCurrentUser();
            PhotoShareContext dbContext = new PhotoShareContext();
            
            if (!Helper.IsValidTags(dbContext, tagNames))
            {
                throw new ArgumentException(Constants.InvalidTags);
            }

            if (Helper.IsAlbumNameExist(dbContext, albumTitle))
            {
                throw new ArgumentException(string.Format(Constants.AlbumExists, albumTitle));
            }

            Color color;
            bool isValidColor = Enum.TryParse(bgColor, out color);
            if (!isValidColor)
            {
                throw new ArgumentException(string.Format(Constants.ColorNotFound, bgColor));
            }

            using (dbContext)
            {
                Album album = new Album()
                {
                    Name = albumTitle,
                    BackgroundColor = color,
                    IsPublic = false
                };
                dbContext.Albums.Add(album);

                AlbumRole albumRole = new AlbumRole()
                {
                    AlbumId = album.Id,
                    UserId = currentUser.Id,
                    Role = Role.Owner
                };
                dbContext.AlbumRoles.Add(albumRole);

                List<AlbumTag> albumTags = new List<AlbumTag>();
                foreach (string tagName in tagNames)
                {
                    AlbumTag albumTag = new AlbumTag()
                    {
                        AlbumId = album.Id,
                        TagId = dbContext.Tags.FirstOrDefault(t => t.Name == $"#{tagName}").Id
                    };

                    albumTags.Add(albumTag);
                }

                dbContext.AlbumTags.AddRange(albumTags);
                dbContext.SaveChanges();
            }

            return string.Format(Constants.CreateAlbum, albumTitle);
        }
    }
}