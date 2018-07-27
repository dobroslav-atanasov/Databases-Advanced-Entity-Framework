namespace BusTicketsSystem.Models
{
    using System.Collections.Generic;

    public class BusCompany
    {
        public BusCompany()
        {
            this.Trips = new List<Trip>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Rating { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}