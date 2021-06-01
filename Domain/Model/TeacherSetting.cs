using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class TeacherSetting
    {
        public Guid Key { get; set; }
        public Guid ServiceTeacherKey { get; set; }
        public int ScheduleTeacherId { get; set; }
        public bool IsEntireAreaShown { get; set; }
    }
}
