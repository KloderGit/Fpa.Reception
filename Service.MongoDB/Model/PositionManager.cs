using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    public class PositionManager
    {
        public PositionTypeDto LimitType;
        public IEnumerable<Position> Positions { get; set; } = new List<Position>();
    }

    public enum PositionTypeDto
    {
        Seating,
        Number,
        Free
    }
}