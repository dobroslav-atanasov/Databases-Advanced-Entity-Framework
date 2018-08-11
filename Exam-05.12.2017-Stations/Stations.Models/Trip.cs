namespace Stations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Trip
    {
        public Trip()
        {
            this.Tickets = new List<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        public int OriginStationId { get; set; }
        public Station OriginStation { get; set; }

        public int DestinationStationId { get; set; }
        public Station DestinationStation { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        public int TrainId { get; set; }
        public Train Train { get; set; }

        [Required]
        public TripStatus Status { get; set; }

        public TimeSpan? TimeDifference { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}