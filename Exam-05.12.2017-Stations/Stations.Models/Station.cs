namespace Stations.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Station
    {
        public Station()
        {
            this.TripsTo = new List<Trip>();
            this.TripsFrom = new List<Trip>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Town { get; set; }

        public ICollection<Trip> TripsTo { get; set; }

        public ICollection<Trip> TripsFrom { get; set; }
    }
}