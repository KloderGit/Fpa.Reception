using System;
using System.Collections.Generic;
using System.Text;

namespace Application.HttpClient
{
    public class ProgramDto
    {
        public Guid Key { get; set; }
        public Guid EducationFormKey { get; set; }

        public IEnumerable<DisciplineInfo> Disciplines { get; set; }
    }

    public class DisciplineInfo
    {
        public Guid DisciplineKey { get; set; }
        public Guid ControlTypeKey { get; set; }
    }

}
