
using System;
using System.Collections.Generic;

namespace Service.lC.Model
{
    public class ControlType : Base
    {
        public IEnumerable<ScoreInfo> RateType { get; set; } = new List<ScoreInfo>();

        public class ScoreInfo
        {
            public string LineNumber { get; set; }
            public ScoreType RateKey { get; set; }
        }
    }
}