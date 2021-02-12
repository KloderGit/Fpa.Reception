using System;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class RestrictionViewModel
    {
        public Guid Program { get; set; }
        public Guid Group { get; set; }
        public Guid SubGroup { get; set; }

        public OptionViewModel Option { get; set; }
    }
}
