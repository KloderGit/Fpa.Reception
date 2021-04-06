using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime SetKind(this DateTime date, DateTimeKind kind)
        {        
            return DateTime.SpecifyKind(date, kind);
        }
    }
}
