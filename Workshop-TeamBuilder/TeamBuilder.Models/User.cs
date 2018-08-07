namespace TeamBuilder.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class User
    {
        public User()
        {
            this.Teams = new List<Team>();
            this.Events = new List<Event>();
            this.Invitations = new List<Invitation>();
            this.UserTeams = new List<UserTeam>();
        }

        public User(string username, string password, string fisrtName, string lastName, int age, Gender gender)
            :this()
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = fisrtName;
            this.LastName = lastName;
            this.Age = age;
            this.Gender = gender;
        }

        public int Id { get; set; }

        [MinLength(3)]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
        
        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }
}