namespace BillsPaymentSystem.SeedData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Generators;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;

    public static class SeedDatabase
    {
        public static void InitilizeDatabase()
        {
            BillsPaymentSystemContext dbContext = new BillsPaymentSystemContext();

            using (dbContext)
            {
                dbContext.Database.EnsureDeleted();

                dbContext.Database.Migrate();

                Seed(dbContext);
            }
        }

        private static void Seed(BillsPaymentSystemContext dbContext)
        {
            SeedUsernames(dbContext);
            SeedBankAccounts(dbContext);
            SeedCreditCards(dbContext);
            SeedPaymentMethods(dbContext);
        }

        private static void SeedPaymentMethods(BillsPaymentSystemContext dbContext)
        {
            User[] users = dbContext.Users.ToArray();
            BankAccount[] bankAccounts = dbContext.BankAccounts.ToArray();
            CreditCard[] creditCards = dbContext.CreditCards.ToArray();

            List<PaymentMethod> paymentMethods = new List<PaymentMethod>();
            for (int i = 0; i < 50; i++)
            {
                PaymentMethod paymentMethod = new PaymentMethod()
                {
                    User = users[i],
                    PaymentType = PaymentType.BankAccount,
                    BankAccount = bankAccounts[i],
                    CreditCard = creditCards[i]
                };
                paymentMethods.Add(paymentMethod);
            }

            dbContext.PaymentMethods.AddRange(paymentMethods);
            dbContext.SaveChanges();
        }

        private static void SeedCreditCards(BillsPaymentSystemContext dbContext)
        {
            List<CreditCard> creditCards = new List<CreditCard>();
            for (int i = 0; i < 50; i++)
            {
                decimal limit = MoneyGenerator.Money;
                decimal moneyOwed = MoneyGenerator.Money;
                if (limit < moneyOwed)
                {
                    while (true)
                    {
                        moneyOwed = MoneyGenerator.Money;
                        if (limit >= moneyOwed)
                        {
                            break;
                        }
                    }
                }
                DateTime expirationDate = DateGenerator.Date;

                CreditCard creditCard = new CreditCard(limit, moneyOwed, expirationDate);
                creditCards.Add(creditCard);
            }

            dbContext.CreditCards.AddRange(creditCards);
            dbContext.SaveChanges();
        }

        private static void SeedBankAccounts(BillsPaymentSystemContext dbContext)
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            for (int i = 0; i < 50; i++)
            {
                string bankName = BankGenerator.BankName;
                string swiftCode = SwiftCodeGenerator.SwiftCode;
                decimal balance = BalanceGenerator.Balance;

                BankAccount bankAccount = new BankAccount(bankName, swiftCode, balance);
                bankAccounts.Add(bankAccount);
            }

            dbContext.BankAccounts.AddRange(bankAccounts);
            dbContext.SaveChanges();
        }

        private static void SeedUsernames(BillsPaymentSystemContext dbContext)
        {
            List<User> users = new List<User>();
            for (int i = 0; i < 50; i++)
            {
                string firstName = NameGenerator.FirstName;
                string lastName = NameGenerator.LastName;
                string email = $"{firstName.ToLower()}.{lastName.ToLower()}{EmailGenerator.Email}";
                string password = "1234";
                User user = new User(firstName, lastName, email, password);
                users.Add(user);
            }

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }
}