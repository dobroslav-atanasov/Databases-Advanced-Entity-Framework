namespace BillsPaymentSystem.SeedData.Generators
{
    using System;
    using System.IO;
    using System.Linq;

    internal class BalanceGenerator
    {
        private static string[] input = File.ReadAllLines("../../../../BillsPaymentSystem.SeedData/Files/Balances.txt");
        private static decimal[] balances = input.Select(b => decimal.Parse(b)).ToArray();

        internal static decimal Balance => GetBalance();

        private static decimal GetBalance()
        {
            Random random = new Random();
            int index = random.Next(0, balances.Length);
            decimal balance = balances[index];

            return balance;
        }
    }
}