using System;

namespace Service.MongoDB.Model
{
    public class PositionPayloadDto
    {
        public Guid StudentKey { get; set; }
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public PositionPayloadResultDto Result { get; set; }
    }
}