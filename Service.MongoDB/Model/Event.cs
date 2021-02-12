using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    public class Event
    {
        public IEnumerable<BaseInfo> Teachers { get; set; } = new List<BaseInfo>();
        public BaseInfo Discipline { get; set; }
        public IEnumerable<Restriction> Restrictions { get; set; } = new List<Restriction>();
        public Requirement Requirement { get; set; }
    }
}
