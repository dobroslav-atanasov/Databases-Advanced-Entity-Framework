namespace BillsPaymentSystem.SeedData.Generators
{
    using System;
    using System.IO;
    using System.Linq;

    internal class MoneyGenerator
    {
        private static string[] input = File.ReadAllLines("../../../../BillsPaymentSystem.SeedData/Files/Money.txt");
        private static decimal[] allMoney = input.Select(m => decimal.Parse(m)).ToArray();

        internal static decimal Money => GetMoney();

        private static decimal GetMoney()
        {
            Random random = new Random();
            int index = random.Next(0, allMoney.Length);
            decimal money = allMoney[index];

            return money;
        }
    }
}