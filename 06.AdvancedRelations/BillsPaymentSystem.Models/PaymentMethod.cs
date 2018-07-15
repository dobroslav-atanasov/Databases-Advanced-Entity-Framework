namespace BillsPaymentSystem.Models
{
    using Enums;

    public class PaymentMethod
    {
        public PaymentMethod()
        {
        }

        public PaymentMethod(PaymentType paymentType, User user, BankAccount bankAccount, CreditCard creditCard)
        {
            this.PaymentType = paymentType;
            this.User = user;
            this.BankAccount = bankAccount;
            this.CreditCard = creditCard;
        }

        public int Id { get; set; }

        public PaymentType PaymentType { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}