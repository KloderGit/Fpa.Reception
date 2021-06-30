using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class RequirementViewModel
    {
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;
        public IEnumerable<BaseInfoViewModel> DependsOnOtherDisciplines { get; set; } = new List<BaseInfoViewModel>();
        public int AllowedAttemptCount { get; set; }
    }
}
