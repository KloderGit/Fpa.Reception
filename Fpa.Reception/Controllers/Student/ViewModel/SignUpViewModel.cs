using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student.ViewModel
{
    public class SignUpViewModel
    {
        public Guid StudentKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public Guid ProgramKey { get; set; }
        public Guid PositionKey { get; set; }
    }
}
