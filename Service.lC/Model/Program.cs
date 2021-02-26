
using System.Collections.Generic;

namespace Service.lC.Model
{
    public class Program : Base
    {
        public EducationForm EducationForm { get; set; }
        public IEnumerable<Base> Teachers { get; set; }
        public IEnumerable<Education> Educations { get; set; }
    }

    public class Education
    {
        public Discipline Discipline { get; set; }
        public ControlType ControlType { get; set; }
    }
}