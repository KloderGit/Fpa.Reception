using System;
using System.Collections.Generic;

namespace Domain
{
    public class Position
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime Time { get; set; }
        public Record Record { get; set; }
        public List<History> Histories { get; set; } = new List<History>();
    }

    public class Record
    {
        public Guid StudentKey { get; set; }
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public Result Result { get; set; }
    }

    public class Result
    {
        public Guid TeacherKey { get; set; }
        //public Score Score { get; set; }

        public Guid RateKey { get; set; }
        public string Comment { get; set; }
    }
}
