using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.lC.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Коллекция содержит элементы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool IsFilled<T>(this IEnumerable<T> array)
        {
            return array != null && array.Any() != false;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> array)
        {
            return array == null || array.Any() == false;
        }
    }
}
