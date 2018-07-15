namespace BillsPaymentSystem.SeedData.Generators
{
    using System;
    using System.IO;

    internal class EmailGenerator
    {
        private static string[] emails = File.ReadAllLines("../../../../BillsPaymentSystem.SeedData/Files/Emails.txt");

        internal static string Email => GetEmail();

        private static string GetEmail()
        {
            Random random = new Random();
            int index = random.Next(0, emails.Length);
            string email = emails[index];

            return email;
        }
    }
}