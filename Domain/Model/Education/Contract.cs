using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Education
{
    public class Contract
    {
        public IEnumerable<Guid> Students { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime StartEducationDate { get; set; }
        public DateTime FinishEducationhDate { get; set; }
        public BaseInfo EducationProgram { get; set; }
        public BaseInfo Group { get; set; }
        public BaseInfo SubGroup { get; set; }
    }
}
