namespace ProductShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.ProductBought = new List<Product>();
            this.ProductSold = new List<Product>();
        }

        public User(string firstName, string lastName, int? age)
            : this()
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
        }

        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        public int? Age { get; set; }

        public virtual ICollection<Product> ProductBought { get; set; }

        public virtual ICollection<Product> ProductSold { get; set; }
    }
}