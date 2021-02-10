using System;

namespace Service.MongoDB.Model
{
    public class PositionPayloadResultDto
    {
        public Guid TeacherKey { get; set; }
        public ScoreDto Score { get; set; }
        public string Comment { get; set; }
    }
}