namespace PhotoShare.App.Utilities
{
    using System;

    internal static class TagUtilities
    {
        private const int DefaultLength = 20;

        public static string ValidateOrTransform(this string wrongTag)
        {
            if (string.IsNullOrWhiteSpace(wrongTag))
            {
                throw new InvalidOperationException("Cannot convert empty string to a valid tag");
            }

            string transformedTag = wrongTag;
            
            transformedTag = RemoveAllWhiteSpaces(transformedTag);

            if (wrongTag[0] != '#')
            {
                transformedTag = AppendPoundSign(transformedTag);
            }

            if (transformedTag.Length > DefaultLength)
            {
                transformedTag = ReduceStringLength(transformedTag, DefaultLength);
            }

            return transformedTag;
        }

        private static string RemoveAllWhiteSpaces(string tag)
        {
            string newTag = tag.Replace(" ", string.Empty)
                .Replace("\t", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty);

            return newTag;
        }

        private static string AppendPoundSign(string tag)
        {
            return "#" + tag;
        }

        private static string ReduceStringLength(string tag, int length)
        {
            string reducedString = tag.Substring(0, length);
            return reducedString;
        }
    }
}