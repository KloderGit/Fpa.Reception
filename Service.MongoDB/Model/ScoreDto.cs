using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Service.MongoDB.Model
{

    public class ScoreDto
    {
        public ScoreTypeDto Type { get; set; } = ScoreTypeDto.NoResult;
        public Tuple<string, Object> Value { get; set; }
    }

    public enum ScoreTypeDto
    {
        NoResult,
        Five,
        Hundred,
        Passed,
        IsVisited,
    }
}