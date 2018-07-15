namespace BillsPaymentSystem.Models
{
    using System;
    using System.Text;

    public class BankAccount 
    {
        public BankAccount()
        {
        }

        public BankAccount(string bankName, string swiftCode, decimal balance)
        {
            this.BankName = bankName;
            this.SwiftCode = swiftCode;
            this.Balance = balance;
        }

        public int BankAccountId { get; set; }

        public decimal Balance { get; set; }

        public string BankName { get; set; }

        public string SwiftCode { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Money cannot be zero or negative!");
            }
            this.Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Money cannot be zero or negative!");
            }
            this.Balance -= amount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"-- ID: {this.BankAccountId}")
                .AppendLine($"--- Balance: {this.Balance:F2}")
                .AppendLine($"--- Bank: {this.BankName}")
                .Append($"--- SWIFT: {this.SwiftCode}");

            return sb.ToString();
        }
    }
}