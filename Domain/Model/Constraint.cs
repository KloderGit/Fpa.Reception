using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Constraint
    {
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public int ActiveForDays { get; set; }
        public int AllowedAttemptCount { get; set; }
        public IEnumerable<Guid> DependsOnOtherDisciplines { get; set; }

        public bool Validate()
        {
            if (DisciplineKey == default) return false;

            return true;
        }
    }
}