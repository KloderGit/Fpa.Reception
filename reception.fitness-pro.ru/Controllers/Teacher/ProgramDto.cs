using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Teacher
{
    public class ProgramDto
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public Guid TypeKey { get; set; }

        public IEnumerable<DisciplineInfo> Disciplines { get; set; }
    }

    public class DisciplineInfo
    {
        public Guid Key { get; set; }
        public Guid ControlTypeKey { get; set; }
        public DisciplineDto Item { get; set; }
    }

    public class DisciplineDto
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }
}
