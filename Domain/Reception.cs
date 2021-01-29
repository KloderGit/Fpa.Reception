using System;
using System.Collections.Generic;

namespace Domain
{
    public class Reception
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> ResponsibleUserKeys { get; set; } = new List<Guid>();
        public List<Guid> DisciplineKeys { get; set; } = new List<Guid>();
        public DateTime Date { get; set; }
        public Constraint Constraints { get; set; }
        public ReceptionLimit Limit { get;set;}
        public List<Position> Positions { get; set; } = new List<Position>();
        public List<History> Histories { get; set; } = new List<History>();
    }

    public class Payload
    {
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public Constraint Constraints { get; set; }
    }


}
