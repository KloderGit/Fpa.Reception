using System;
using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    public class PositionDto
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime Time { get; set; }
        public PositionPayloadDto Payload { get; set; }
        public IEnumerable<HistoryDto> Histories { get; set; } = new List<HistoryDto>();
    }
}