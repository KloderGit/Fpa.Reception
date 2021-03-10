using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Model
{
    public class Contract : Base
    {
        public IEnumerable<Guid> Students { get; set; }

        public DateTime ExpiredDate { get; set; }
        public DateTime StartEducationDate { get; set; }
        public DateTime FinisEducationhDate { get; set; }
        public Guid EducationProgramKey { get; set; }
        public Guid GroupKey { get; set; }
        public Guid SubGroupKey { get; set; }
    }
}
