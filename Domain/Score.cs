using System;

namespace Domain
{
    public class Score
    {
        public ScoreType Type { get; set; } = ScoreType.NoResult;
        public virtual Tuple<Type, Object> Value { get; set; }
        public Score()
        {
            //Type = type;
        }
    }

    public class Five : Score
    {
        public Five(int value)
            //: base(ScoreType.Five)
        {
            if(value < 0 && value > 5) throw new ArgumentException("Значение оценки выходит за пятибальный диапазон");
            Value = new Tuple<Type, object>(value.GetType(), value);
        }
    }

    public class Hundred : Score
    {
        public Hundred(int value)
            // base(ScoreType.Hundred)
        {
            if (value < 0 && value > 100) throw new ArgumentException("Значение оценки выходит за стобальный диапазон");
            Value = new Tuple<Type, object>(value.GetType(), value);
        }
    }

    public class Passed : Score
    {
        public Passed(bool value)
           // : base(ScoreType.Passed)
        {            
            Value = new Tuple<Type, object>(value.GetType(), value);
        }
    }

    public class IsVisited : Score
    {
        public IsVisited(bool value)
           // : base(ScoreType.IsVisited)
        {
            Value = new Tuple<Type, object>(value.GetType(), value);
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
