namespace BillsPaymentSystem.SeedData.Generators
{
    using System;
    using System.IO;

    internal class DateGenerator
    {
        private static string[] dates = File.ReadAllLines("../../../../BillsPaymentSystem.SeedData/Files/Dates.txt");

        internal static DateTime Date => GetDate();

        private static DateTime GetDate()
        {
            Random random = new Random();
            int index = random.Next(0, dates.Length);
            DateTime date = DateTime.ParseExact(dates[index], "yyyy-MM-dd", null);

            return date;
        }
    }
}