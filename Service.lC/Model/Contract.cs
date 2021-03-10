using System;
using System.Collections.Generic;

namespace Service.lC.Model
{
    public class Contract : Base
    {
        public IEnumerable<Guid> Students { get; set; }

        public DateTime ExpiredDate { get; set; }
        public DateTime StartEducationDate { get; set; }
        public DateTime FinishEducationhDate { get; set; }
        public Base EducationProgram { get; set; }
        public Base Group { get; set; }
        public Base SubGroup { get; set; }
    }
}
