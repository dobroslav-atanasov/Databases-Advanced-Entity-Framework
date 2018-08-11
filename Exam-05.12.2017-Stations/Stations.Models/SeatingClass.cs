namespace Stations.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SeatingClass
    {
        public SeatingClass()
        {
            this.TrainSeats = new List<TrainSeat>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Abbreviation { get; set; }

        public ICollection<TrainSeat> TrainSeats { get; set; }
    }
}