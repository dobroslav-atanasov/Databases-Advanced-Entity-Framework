namespace BusTicketsSystem.Models
{
    using System.Collections.Generic;

    public class BusStation
    {
        public BusStation()
        {
            this.OriginTrips = new List<Trip>();
            this.DestinationTrips = new List<Trip>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public virtual ICollection<Trip> OriginTrips { get; set; }

        public virtual ICollection<Trip> DestinationTrips { get; set; }
    }
}