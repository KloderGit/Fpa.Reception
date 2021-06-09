using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Education
{
    public class Program : BaseInfo
    {
        public BaseInfo EducationForm { get; set; }
        public IEnumerable<BaseInfo> Teachers { get; set; } = new List<BaseInfo>();
        public IEnumerable<ProgramEducation> Educations { get; set; } = new List<ProgramEducation>();
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();

        public Guid? FindControlTypeKey(Guid disciplineKey)
        {
            var education = Educations.FirstOrDefault(x => x.Discipline.Key == disciplineKey);
            if (education == default) return null;

            var controlType = education.ControlType;

            return controlType.Key;
        }

        public Guid GetControlTypeKey(Guid disciplineKey)
        {
            var education = Educations.FirstOrDefault(x => x.Discipline.Key == disciplineKey);
            var controlType = education.ControlType;

            return controlType.Key;
        }
    }

    public class ProgramEducation
    {
        public int Order { get; set; }
        public BaseInfo Discipline { get; set; }
        public ControlType ControlType { get; set; }
    }
}
