using reception.fitnesspro.ru.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class EventViewModel
    {
        [Required]
        public IEnumerable<BaseInfoViewModel> Teachers { get; set; } = new List<BaseInfoViewModel>();
        [Required]
        public BaseInfoViewModel Discipline { get; set; }
        public IEnumerable<ConstraintViewModel> Constraints { get; set; } = new List<ConstraintViewModel>();
        public RequirementViewModel Requirement { get; set; }
    }
}
