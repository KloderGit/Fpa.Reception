using System;
using System.Collections.Generic;
using System.Text;

namespace Application.HttpClient
{
    public class BasicEntity
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }

    public class ProgramDto
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public Guid TypeKey { get; set; }
        public BasicEntity EducationForm { get; set; }

        public IEnumerable<DisciplineInfo> Disciplines { get; set; }
    }

    public class DisciplineInfo
    {
        public Guid Key { get; set; }
        public Guid ControlTypeKey { get; set; }
        public DisciplineDto Item { get; set; }
    }

    public class DisciplineDto : BasicEntity
    {}
}
