namespace Stations.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Train
    {
        public Train()
        {
            this.TrainSeats = new List<TrainSeat>();
            this.Trips = new List<Trip>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string TrainNumber { get; set; }

        public TrainType? Type { get; set; }

        public ICollection<TrainSeat> TrainSeats { get; set; }

        public ICollection<Trip> Trips { get; set; }
    }
}