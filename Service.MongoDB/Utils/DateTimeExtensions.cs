using System;
using System.Collections.Generic;
using System.Text;

namespace Service.MongoDB.Utils
{
    public static class DateTimeExtensions
    {
        public static DateTime SetKind(this DateTime date, DateTimeKind kind)
        {        
            var newdate = DateTime.SpecifyKind(date, kind);
            return newdate;
        }
    }
}
