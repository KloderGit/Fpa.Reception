using System;
using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    public class Requirement
    {
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;
        public IEnumerable<Guid> DependsOnOtherDisciplines { get; set; } = new List<Guid>();
        public int AllowedAttemptCount { get; set; }
    }
}