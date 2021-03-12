using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotEmpty(this string @string)
        {
            return String.IsNullOrEmpty(@string) == false;
        }
    }
}
