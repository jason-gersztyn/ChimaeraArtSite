using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Common
{
    public static class Extensions
    {
        public static bool IsNull<T>(this T value)
        {
            return value == null;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static void MustNotBeNull<T>(this T value)
        {
            if(value.IsNull())
            {
                throw new ArgumentNullException();
            }
        }

        public static void MustNotBeNullOrWhiteSpace(this string value)
        {
            if(value.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Value cannot be null or whitespace.");
            }
        }
    }
}
