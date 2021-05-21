using System;

namespace Service.Schedule.MySql.Model
{
    public class EventInfo
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
}