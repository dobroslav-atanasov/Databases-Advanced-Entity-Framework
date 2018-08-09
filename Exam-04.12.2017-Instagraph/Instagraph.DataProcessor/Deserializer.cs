using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.Models;

namespace Instagraph.DataProcessor
{
    using DtoModels;

    public class Deserializer
    {
        private static string successMessage = "Successfully imported {0}.";
        private static string errorMessage = "Error: Invalid data.";

        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            PictureDto[] importedPictureDtos = JsonConvert.DeserializeObject<PictureDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Picture> pictures = new List<Picture>();

            foreach (PictureDto dto in importedPictureDtos)
            {
                bool isValid = !string.IsNullOrWhiteSpace(dto.Path) && dto.Size > 0;
                bool isExist = context.Pictures.Any(p => p.Path == dto.Path) &&
                               pictures.Any(p => p.Path == dto.Path);

                if (!isValid || isExist)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                Picture picture = new Picture()
                {
                    Path = dto.Path,
                    Size = dto.Size
                };

                pictures.Add(picture);
                sb.AppendLine(string.Format(successMessage, $"Picture {dto.Path}"));
            }

            context.Pictures.AddRange(pictures);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            UserDto[] importedUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<User> users = new List<User>();

            foreach (UserDto userDto in importedUsers)
            {
                bool isValidUsername = !string.IsNullOrWhiteSpace(userDto.Username)
                                       && userDto.Username.Length <= 30;
                bool isValidPassword = !string.IsNullOrWhiteSpace(userDto.Password)
                                       && userDto.Password.Length <= 20;
                bool isValidPicture = !string.IsNullOrWhiteSpace(userDto.ProfilePicture);
                Picture picture = context.Pictures.FirstOrDefault(p => p.Path == userDto.ProfilePicture);
                bool userExists = context.Users.Any(u => u.Username == userDto.Username)
                                  || users.Any(u => u.Username == userDto.Username);

                if (!isValidUsername || !isValidPassword || !isValidPicture || picture == null || userExists)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                User user = new User()
                {
                    Username = userDto.Username,
                    Password = userDto.Password,
                    ProfilePicture = context.Pictures.FirstOrDefault(p => p.Path == userDto.ProfilePicture)
                };
                users.Add(user);
                sb.AppendLine(string.Format(successMessage, $"User {user.Username}"));
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            UserFollowerDto[] importedUsersFollowers = JsonConvert.DeserializeObject<UserFollowerDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<UserFollower> usersFollowers = new List<UserFollower>();

            foreach (UserFollowerDto dto in importedUsersFollowers)
            {
                User user = context.Users.FirstOrDefault(u => u.Username == dto.User);
                User follower = context.Users.FirstOrDefault(u => u.Username == dto.Follower);
                bool alreadyFollowed = usersFollowers.Any(f => f.User == user && f.Follower == follower);

                if (user == null || follower == null || alreadyFollowed)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                UserFollower userFollower = new UserFollower()
                {
                    User = user,
                    Follower = follower
                };

                usersFollowers.Add(userFollower);
                sb.AppendLine(string.Format(successMessage, $"Follower {userFollower.Follower.Username} to User {userFollower.User.Username}"));
            }

            context.UsersFollowers.AddRange(usersFollowers);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            XDocument xDoc = XDocument.Parse(xmlString);
            var elements = xDoc.Root.Elements();
            StringBuilder sb = new StringBuilder();
            List<Post> posts = new List<Post>();

            foreach (XElement element in elements)
            {
                string caption = element.Element("caption")?.Value;
                string username = element.Element("user")?.Value;
                string picturePath = element.Element("picture")?.Value;

                bool isValid = !string.IsNullOrWhiteSpace(caption) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(picturePath);
                User user = context.Users.FirstOrDefault(u => u.Username == username);
                Picture picture = context.Pictures.FirstOrDefault(p => p.Path == picturePath);

                if (!isValid ||user == null || picture == null)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                Post post = new Post()
                {
                    Caption = caption,
                    User = user,
                    Picture = picture
                };

                posts.Add(post);
                sb.AppendLine(string.Format(successMessage, $"Post {post.Caption}"));
            }

            context.Posts.AddRange(posts);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            XDocument xDoc = XDocument.Parse(xmlString);
            var elements = xDoc.Root.Elements();
            StringBuilder sb = new StringBuilder();
            List<Comment> comments = new List<Comment>();

            foreach (XElement element in elements)
            {
                string content = element.Element("content")?.Value;
                string username = element.Element("user")?.Value;
                string postIdString = element.Element("post")?.Attribute("id")?.Value;

                bool isValid = !string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(postIdString);

                if (!isValid)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                User user = context.Users.FirstOrDefault(u => u.Username == username);
                Post post = context.Posts.FirstOrDefault(p => p.Id == int.Parse(postIdString));
                
                if (user == null || post == null)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                Comment comment = new Comment()
                {
                    Content = content,
                    User = user,
                    Post = post
                };

                comments.Add(comment);
                sb.AppendLine(string.Format(successMessage, $"Comment {content}"));
            }

            context.Comments.AddRange(comments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }
    }
}
