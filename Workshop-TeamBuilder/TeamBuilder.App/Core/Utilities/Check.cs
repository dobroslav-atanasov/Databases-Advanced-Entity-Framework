namespace TeamBuilder.App.Core.Utilities
{
    using System;
    using Services.Utilities;

    public static class Check
    {
        public static void CheckLength(int expectedLength, string[] array)
        {
            if (expectedLength != array.Length)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }
        }
    }
}