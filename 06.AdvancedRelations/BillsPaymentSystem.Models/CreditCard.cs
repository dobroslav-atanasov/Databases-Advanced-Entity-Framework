namespace BillsPaymentSystem.Models
{
    using System;
    using System.Text;

    public class CreditCard
    {
        public CreditCard()
        {
        }

        public CreditCard(decimal limit, decimal moneyOwed, DateTime expirationDate)
        {
            this.Limit = limit;
            this.MoneyOwed = moneyOwed;
            this.ExpirationDate = expirationDate;
        }

        public int CreditCardId { get; set; }

        public decimal Limit { get; set; }

        public decimal MoneyOwed { get; set; }

        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        public DateTime ExpirationDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Money cannot be zero or negative!");
            }
            this.MoneyOwed += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Money cannot be zero or negative!");
            }
            this.MoneyOwed -= amount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"-- ID: {this.CreditCardId}")
                .AppendLine($"--- Limit: {this.Limit:F2}")
                .AppendLine($"--- Money Owed: {this.MoneyOwed:F2}")
                .AppendLine($"--- Limit Left:: {this.LimitLeft:F2}")
                .Append($"--- Expiration Date: {this.ExpirationDate.Year}/{this.ExpirationDate.Month}");

            return sb.ToString();
        }
    }
}