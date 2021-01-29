using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Event
    {
        public Guid Key { get; set; }
        public string Title { get; set; }

        public EventLimit Limit { get; set; } = new EventLimit();

        //public ResultType ResultType { get; set; }
    }

    public class EventLimit
    {
        public List<Guid> DependsOn { get; set; } = new List<Guid>();
        public int AttemptNumbers { get; set; } = default;
    }

    public enum ResultType
    {
        Five,
        Hundred,
        Passed,
    }
}
