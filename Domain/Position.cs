using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Position
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public (int,int) Time { get; set; }
        public Guid StudentKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public Score Score { get; set; } = new Score(ScoreType.NoResult);
        public string Comment { get; set; }
        public List<History> Histories { get; set; } = new List<History>();
    }
}
