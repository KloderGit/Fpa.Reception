using System.Collections.Generic;

namespace Domain.Model
{
    public class ScheduleProgramInfo : BaseSchedule
    {
        public IEnumerable<BaseSchedule> Groups { get; set; }
    }
}
