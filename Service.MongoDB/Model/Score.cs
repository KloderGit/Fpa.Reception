using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Service.MongoDB.Model
{

    public class Score
    {
        public ScoreType Type { get; set; } = ScoreType.NoResult;
        public Tuple<string, Object> Value { get; set; }
    }

    public enum ScoreType
    {
        NoResult,
        Five,
        Hundred,
        Passed,
        IsVisited,
    }
}