using System;
using System.Collections.Generic;

namespace Domain
{
    public class Reception
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public PositionManager PositionManager { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();

        public List<History> Histories { get; set; } = new List<History>();

        public TConverted ConvertToType<TConverted>(Func<Reception, TConverted> function )
        {
            var result = function(this);

            return result;
        }

        public Reception ConvertFromType<TConverted>(Func<TConverted, Reception> function, TConverted item)
        {
            var result = function(item);

            return result;
        }
    }

    public class PositionManager
    {
        public PositionType LimitType;
        public PositionManager()
        {
        }
        public List<Position> Positions { get; set; } = new List<Position>();
    }

    public enum PositionType
    {
        Seating,
        Number,
        Free
    }


    public class Event
    {
        public List<BaseInfo> Teachers { get; set; } = new List<BaseInfo>();
        public BaseInfo Discipline { get; set; }
        public List<PayloadRestriction> Restrictions { get; set; } = new List<PayloadRestriction>();
        public PayloadRequirement Requirement { get; set; }
    }

    public class PayloadRequirement
    {
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;

        public IEnumerable<Guid> DependsOnOtherDisciplines { get; set; } = new List<Guid>();
        public int AllowedAttemptCount { get; set; }
    }

    public class PayloadRestriction
    {
        public Guid Program { get; set; }
        public Guid Group { get; set; }
        public Guid SubGroup { get; set; }

        public PayloadOption Option { get; set; }
    }

    public class PayloadOption
    {
        public bool CheckContractExpired { get; set; } = true;
        public bool CheckDependings { get; set; } = true;
        public bool CheckAttemps { get; set; } = true;
    }

}
