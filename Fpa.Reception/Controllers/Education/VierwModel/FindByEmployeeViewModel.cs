using reception.fitnesspro.ru.ViewModel;
using System.Collections.Generic;

namespace reception.fitnesspro.ru.Controllers.Education.ViewModel
{
    public class FindByEmployeeViewModel : BaseInfoViewModel
    {
        public BaseInfoViewModel EducationForm { get; set; }
        public IEnumerable<BaseInfoViewModel> Teachers { get; set; }
        public IEnumerable<Education> Disciplines { get; set; }
        public IEnumerable<Group> Groups { get; set; }

        public class Education : BaseInfoViewModel
        {
            BaseInfoViewModel ControlType { get; set; }
        }

        public class Group : BaseInfoViewModel
        {
            IEnumerable<BaseInfoViewModel> SubGroups { get; set; }
        }
    }
}
