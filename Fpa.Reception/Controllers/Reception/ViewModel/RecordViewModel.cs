using System;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class RecordViewModel
    {
        public Guid StudentKey { get; set; }
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public ResultViewModel Result { get; set; }
    }
}