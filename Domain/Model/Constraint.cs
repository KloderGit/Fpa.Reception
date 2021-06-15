using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public abstract class Constraint : KeyEntity
    {
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }

        [Obsolete]
        public virtual bool Validate()
        {
            if (DisciplineKey == default) return false;

            return true;
        }
    }

    public class BaseConstraint : Constraint
    {
        public int AllowedAttempts { get; set; }
        public IEnumerable<BaseInfo> DependsOn { get; set; }
        public bool CheckContract { get; set; }
        public int SignUpBeforeMinutes { get; set; }
        public int SignOutBeforeMinutes { get; set; }

        public UISettings UISettings { get; set; }
    }

    public class UISettings
    {
        public IEnumerable<PositionType> PositionTypes { get; set; }
        public IEnumerable<BaseInfo> Teachers { get; set; }
        public int OncePerMinutes { get; set; }
        public int StudentsNumber { get; set; }
    }

    public class GroupSettings
    {
        public Guid Key { get; set; }
        public BaseInfo Program { get; set; }
        public BaseInfo Group { get; set; }
        public string DiscordLink { get; set; }
        public int ScheduleGroupId { get; set; }
        public IEnumerable<EventPeriodConstraint> DisciplineLimits { get;set; }

        public class EventPeriodConstraint
        {
            public BaseInfo Discipline { get; set; }
            public DateTime StartPeriod { get; set; }
            public DateTime FinishPeriod { get; set; }
        }
    }
}