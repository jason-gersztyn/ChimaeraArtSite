using System;

namespace Chimaera.Beasts.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNotEmpty(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static DateTime ToDateTime(this string input)
        {
            DateTime result;
            if (!DateTime.TryParse(input, out result))
                result = DateTime.MinValue;
            return result;
        }
    }
}