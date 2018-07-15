namespace BillsPaymentSystem.SeedData.Generators
{
    using System;
    using System.IO;

    internal class BankGenerator
    {
        private static string[] banks = File.ReadAllLines("../../../../BillsPaymentSystem.SeedData/Files/Banks.txt");

        internal static string BankName => GetBankName();

        private static string GetBankName()
        {
            Random random = new Random();
            int index = random.Next(0, banks.Length);
            string bankName = banks[index];

            return bankName;
        }
    }
}