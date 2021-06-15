using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class TeacherSetting : KeyEntity
    {
        public Guid ServiceTeacherKey { get; set; }
        public int ScheduleTeacherId { get; set; }
        public bool IsEntireAreaShown { get; set; }
    }
}
