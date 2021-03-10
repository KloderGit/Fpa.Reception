using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ReceptionLimit
    {
        public PositionType Type {get; private set;}
        public int Quantity { get; private set; }

        public ReceptionLimit(PositionType type)
        {
            Type = type;
        }
        public ReceptionLimit(PositionType type, int quantity)
            :this(PositionType.Number)
        {
            Quantity = quantity;
        }

        public bool CanSubscribe()
        { 
            return true;
        }
    }

}