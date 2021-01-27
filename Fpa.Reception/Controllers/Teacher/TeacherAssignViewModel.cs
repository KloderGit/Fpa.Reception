using System;
using System.Collections.Generic;

namespace reception.fitnesspro.ru.Controllers.Teacher
{
    public class TeacherAssignViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }

        public IEnumerable<ProgramInfoViewModel> Programs { get; set; } = new List<ProgramInfoViewModel>();
    }

    public class ProgramInfoViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }

        public EducationFormViewModel EducationForm { get; set; }

        public IEnumerable<DisciplinePlanViewModel> Disciplines { get; set; } = new List<DisciplinePlanViewModel>();

    }

    public class EducationFormViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }

    public class DisciplinePlanViewModel
    {
        public DisciplineViewModel Discipline { get; set; }
        public ControlTypeViewModel ControlType { get; set; }
    }

    public class DisciplineViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }

    public class ControlTypeViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }
}
