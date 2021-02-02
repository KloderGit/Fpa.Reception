using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Position
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public (int, int) Time { get; set; }
        public PositionPayload Payload { get; set; }
        public List<History> Histories { get; set; } = new List<History>();
    }

    public class PositionPayload
    {
        public Guid StudentKey { get; set; }
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        //public ScoreType ScoreType { get; set; }
        public PositionPayloadResult Result { get; set; }
    }

    public class PositionPayloadResult
    {
        public Guid TeacherKey { get; set; }
        public Score Score { get; set; }
        public string Comment { get; set; }
    }
}
