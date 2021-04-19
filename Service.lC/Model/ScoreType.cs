using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Model
{
    public class ScoreType : Base
    {
        public Guid ParentKey { get; set; }
        public int MaxGrade { get; set; }
        public int MinGrade { get; set; }
        public string Grade { get; set; }

        public IEnumerable<Base> ScoreVariants { get; set; } = new List<Base>();
    }
}
