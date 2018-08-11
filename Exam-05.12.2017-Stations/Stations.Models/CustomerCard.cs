namespace Stations.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class CustomerCard
    {
        public CustomerCard()
        {
            this.BoughtTickets = new List<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        [Required]
        public CardType Type { get; set; }

        public ICollection<Ticket> BoughtTickets { get; set; }
    }
}