namespace BillsPaymentSystem.SeedData.Generators
{
    using System;
    using System.IO;

    internal class SwiftCodeGenerator
    {
        private static string[] swiftCodes = File.ReadAllLines("../../../../BillsPaymentSystem.SeedData/Files/SwiftCodes.txt");

        internal static string SwiftCode => GetSwiftCode();

        private static string GetSwiftCode()
        {
            Random random = new Random();
            int index = random.Next(0, swiftCodes.Length);
            string swiftCode = swiftCodes[index];

            return swiftCode;
        }
    }
}