using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class PositionViewModel
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime Time { get; set; }
        public RecordViewModel Record { get; set; }
    }
}
