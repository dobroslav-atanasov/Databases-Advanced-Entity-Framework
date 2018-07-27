namespace BusTicketsSystem.Models
{
    using System.Collections.Generic;

    public class Town
    {
        public Town()
        {
            this.BusStations = new List<BusStation>();
            this.Customers = new List<Customer>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public virtual ICollection<BusStation> BusStations { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}