namespace BusTicketsSystem.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        public decimal Balance { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}