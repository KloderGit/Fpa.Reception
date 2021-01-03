using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Score
    {
        public ScoreType Type { get; set; } = ScoreType.NoResult;
        public virtual (Type, Object) Value { get; set; }
        public Score(ScoreType type)
        {
            Type = type;
        }
    }

    public class Five : Score
    {
        public Five(int value)
            : base(ScoreType.Five)
        {
            if(value < 0 && value > 5) throw new ArgumentException("Значение оценки выходит за пятибальный диапазон");
            Value = (value.GetType(), value);
        }
    }

    public class Hundred : Score
    {
        public Hundred(int value)
            : base(ScoreType.Hundred)
        {
            if (value < 0 && value > 100) throw new ArgumentException("Значение оценки выходит за стобальный диапазон");
            Value = (value.GetType(), value);
        }
    }

    public class Passed : Score
    {
        public Passed(bool value)
            : base(ScoreType.Passed)
        {            
            Value = (value.GetType(), value);
        }
    }

    public class IsVisited : Score
    {
        public IsVisited(bool value)
            : base(ScoreType.IsVisited)
        {
            Value = (value.GetType(), value);
        }
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
