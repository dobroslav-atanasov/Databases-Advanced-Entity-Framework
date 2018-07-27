namespace BusTicketsSystem.Models
{
    using System;

    public class Review
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int BusCompanyId { get; set; }
        public virtual BusCompany BusCompany { get; set; }

        public string Content { get; set; }

        public int Grade { get; set; }

        public DateTime Published { get; set; }
    }
}