using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Model
{
    public class ScoreType : Base
    {
        public int MinRate { get; set; }
        public int MaxRate { get; set; }

        public string Rate { get; set; }
    }
}
