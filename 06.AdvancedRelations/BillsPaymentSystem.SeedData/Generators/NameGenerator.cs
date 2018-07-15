namespace BillsPaymentSystem.SeedData.Generators
{
    using System;
    using System.IO;

    internal static class NameGenerator
    {
        private static string[] firstNames = File.ReadAllLines("../../../../BillsPaymentSystem.SeedData/Files/FirstNames.txt");
        private static string[] lastNames = File.ReadAllLines("../../../../BillsPaymentSystem.SeedData/Files/LastNames.txt");

        internal static string FirstName => GetName(firstNames);
        internal static string LastName => GetName(lastNames);

        private static string GetName(string[] names)
        {
            Random random = new Random();
            int index = random.Next(0, names.Length);
            string name = names[index];

            return name;
        }
    }
}