using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    public class PositionManagerDto
    {
        public PositionTypeDto LimitType;
        public IEnumerable<PositionDto> Positions { get; set; } = new List<PositionDto>();
    }

    public enum PositionTypeDto
    {
        Seating,
        Number,
        Free
    }
}