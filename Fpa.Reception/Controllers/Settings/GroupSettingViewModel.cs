using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Settings
{
    public class GroupSettingViewModel
    {
        public Guid Key { get; set; }
        [Required]
        public BaseInfoViewModel Program { get; set; }
        [Required]
        public BaseInfoViewModel Group { get; set; }
        public string DiscordLink { get; set; }
        public int ScheduleGroupId { get; set; }
        public IEnumerable<EventPeriodConstraint> DisciplineLimits { get; set; }

        public class EventPeriodConstraint
        {
            [Required]
            public BaseInfoViewModel Discipline { get; set; }
            public DateTime StartPeriod { get; set; }
            public DateTime FinishPeriod { get; set; }
        }
    }

    public class GroupFromServiceViewModel : BaseInfoViewModel
    {
        public IEnumerable<BaseInfoViewModel> Groups { get; set; }
    }
}
