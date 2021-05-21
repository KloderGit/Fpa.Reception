using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public abstract class Constraint
    {
        public Guid Key { get; set; }
        
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
        public PositionType PositionType { get; set; }
        public IEnumerable<BaseInfo> Teachers { get; set; }
        public int OncePerMinutes { get; set; }
        public int StudentsNumber { get; set; }
    }

    public class GroupConstraint : Constraint
    {
        public Guid GroupKey { get; set; }
        public DateTime AfterDate { get; set; }
        public DateTime BeforeDate { get; set; }
    }
}