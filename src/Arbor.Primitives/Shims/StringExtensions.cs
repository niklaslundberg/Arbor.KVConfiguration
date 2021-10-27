using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NET5_0_OR_GREATER
#else
namespace System
{
    public static class ShimStringExtensions
    {
        public static bool Contains(this string stringValue, string? value, StringComparison stringComparison)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return stringValue.IndexOf(value!, stringComparison) >= 0;
        }
    }
}

#endif