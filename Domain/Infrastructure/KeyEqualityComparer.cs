using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Infrastructure
{
    public class KeyEqualityComparer<T> : IEqualityComparer<T> where T : KeyEntity
    {
        public bool Equals(T x, T y)
        {
            return x.Key == y.Key;
        }

        public int GetHashCode(T obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
