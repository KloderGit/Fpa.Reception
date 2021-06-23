using System;
using System.Collections.Generic;

namespace Service.Schedule.MySql.Model
{
    public class TeacherEventInfo
    {
        public string Title { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime BeginDate { get; set; }
        public string MonthNumber { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string Groups { get; set; }
        public string Place { get; set; }
        public string Education { get; set; }
    }

    public class GroupEventInfo
    {
        public string Title { get; set; }
        public IEnumerable<string> Teachers { get; set; }
        public string Place { get; set; }
        public DateTime BeginDate { get; set; }
        public string MonthNumber { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public int SubGroups { get; set; }
        public bool IsCanceled { get; set; }
    }
}