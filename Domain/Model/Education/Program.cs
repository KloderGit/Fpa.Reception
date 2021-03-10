using System.Collections.Generic;

namespace Domain.Education
{
    public class Program : BaseInfo
    {
        public BaseInfo EducationForm { get; set; }
        public IEnumerable<BaseInfo> Teachers { get; set; } = new List<BaseInfo>();
        public IEnumerable<ProgramEducation> Educations { get; set; } = new List<ProgramEducation>();
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();
    }

    public class ProgramEducation
    {
        public int Order { get; set; }
        public BaseInfo Discipline { get; set; }
        public BaseInfo ControlType { get; set; }
    }
}
