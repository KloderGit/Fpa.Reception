using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Settings
{
    public class ProgramInfoViewModel: BaseInfoViewModel
    {
        public IEnumerable<BaseInfoViewModel> Educations { get; set; } = new List<BaseInfoViewModel>();
        public IEnumerable<BaseInfoViewModel> Groups { get; set; } = new List<BaseInfoViewModel>();        
    }
}
