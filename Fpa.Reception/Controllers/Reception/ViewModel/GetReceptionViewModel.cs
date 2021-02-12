using Domain;
using System;
using System.Collections.Generic;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class GetReceptionViewModel
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<EventViewModel> Events { get; set; }
        public PositionType Type { get; set; }
        public IEnumerable<PositionViewModel> Positions { get; set; }
    }
}
