using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Constraint
    {
        public List<Guid> GroupKeys { get; set; } = new List<Guid>();
        public List<Guid> SubGroupKeys { get; set; } = new List<Guid>();
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;
        public bool CheckContractExpired { get; set; } = default;
    }
}
