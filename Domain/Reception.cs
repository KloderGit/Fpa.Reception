using System;
using System.Collections.Generic;

namespace Domain
{
    public class Reception
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<Payload> Payload { get; set; } = new List<Payload>();
        public PositionManager PositionManager { get; set; }
        public List<History> Histories { get; set; } = new List<History>();
    }

    public class PositionManager
    {
        PositionType limitType;
        public PositionManager(PositionType limitType)
        {
            this.limitType = limitType;
        }
        public IEnumerable<Position> Positions { get; set; } = new List<Position>();
    }

    public enum PositionType
    {
        Seating,
        Number,
        Free
    }


    public class Payload
    {
        public IEnumerable<BaseInfo> Teachers { get; set; } = new List<BaseInfo>();
        public BaseInfo Discipline { get; set; }
        public IEnumerable<PayloadBound> Bound { get; set; } = new List<PayloadBound>();
        public PayloadConstrait Constrait { get; set; }
    }

    public class PayloadConstrait
    {
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;

        public IEnumerable<Guid> DependsOnOtherDiscipline { get; set; } = new List<Guid>();
        public int AllowedAttempCount { get; set; }
    }

    public class PayloadBound
    {
        public Guid Program { get; set; }
        public Guid Group { get; set; }
        public Guid SubGroup { get; set; }

        public PayloadRules Rules { get; set; }
    }

    public class PayloadRules
    {
        public bool CheckContractExpired { get; set; } = true;
        public bool CheckDependings { get; set; } = true;
        public bool CheckAttemps { get; set; } = true;
    }

}
