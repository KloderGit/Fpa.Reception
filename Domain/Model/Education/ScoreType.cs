using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Education
{
    public class ControlType : BaseInfo
    { 
        public IEnumerable<ScoreInfo> RateType { get; set; }
    }

    public class ScoreInfo
    {
        public string LineNumber { get; set; }
        public ScoreType RateKey { get; set; }
    }

    public class ScoreType : BaseInfo
    {
        public Guid ParentKey { get; set; }
        public int MaxGrade { get; set; }
        public int MinGrade { get; set; }
        public string Grade { get; set; }

        public IEnumerable<BaseInfo> ScoreVariants { get; set; }
    }
}
