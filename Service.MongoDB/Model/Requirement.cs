using System;
using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    public class Requirement
    {
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;
        public IEnumerable<Guid> DependsOnOtherDiscipline { get; set; } = new List<Guid>();
        public int AllowedAttempCount { get; set; }
    }
}