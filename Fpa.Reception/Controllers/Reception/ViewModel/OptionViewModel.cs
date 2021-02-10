using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class OptionViewModel
    {
        public bool CheckContractExpired { get; set; }
        public bool CheckDependencies { get; set; }
        public bool CheckAttemps { get; set; }
    }
}
