using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Constraint
    {
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;
    }
}
