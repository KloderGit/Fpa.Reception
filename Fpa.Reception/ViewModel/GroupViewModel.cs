using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.ViewModel
{
    public class GroupViewModel : BaseInfoViewModel
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        //public IEnumerable<SubGroup> SubGroups { get; set; }
    }
}
