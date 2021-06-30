using reception.fitnesspro.ru.ViewModel;
using System;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class RestrictionViewModel
    {
        public BaseInfoViewModel Program { get; set; }
        public BaseInfoViewModel Group { get; set; }
        public BaseInfoViewModel SubGroup { get; set; }

        public OptionViewModel Option { get; set; }
    }
}
