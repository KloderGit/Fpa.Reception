using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ReceptionLimit
    {
        public LimitType Type {get; private set;}
        public int Quantity { get; private set; }

        public ReceptionLimit(LimitType type)
        {
            Type = type;
        }
        public ReceptionLimit(LimitType type, int quantity)
            :this(LimitType.Number)
        {
            Quantity = quantity;
        }

        public bool CanSubscribe()
        { 
            return true;
        }
    }

    public enum LimitType
    {
        Seating,
        Number,
        Free
    }
}