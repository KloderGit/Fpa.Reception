using System;

namespace Domain
{
    public class Score
    {
        public AttestationScoreType Type { get; set; } = AttestationScoreType.NoResult;
        protected virtual Tuple<Type, Object> ScoreValue { get; set; }
        public string Value => ScoreValue?.Item2.ToString();

        public Score(){}
        public Score(AttestationScoreType type)
        {
            Type = type;
        }

        public Score(AttestationScoreType type, Tuple<Type, Object> value)
        {
            Type = type;
            ScoreValue = value;
        }

        public AttestationScoreType GetAttestationScoreType() => Type;
        public Type GetScoreType() => ScoreValue.Item1;
        public Object GetScoreValue() => ScoreValue.Item2;
        public void SetScoreValue(Tuple<Type, Object> value)
        {
            ScoreValue = value;
        }
    }

    public class Five : Score
    {
        public Five(int value)
            : base(AttestationScoreType.Five)
        {
            if(value < 0 && value > 5) throw new ArgumentException("Значение оценки выходит за пятибальный диапазон");
            ScoreValue = new Tuple<Type, object>(value.GetType(), value);
        }
    }

    public class Hundred : Score
    {
        public Hundred(int value)
            : base(AttestationScoreType.Hundred)
        {
            if (value < 0 && value > 100) throw new ArgumentException("Значение оценки выходит за стобальный диапазон");
            ScoreValue = new Tuple<Type, object>(value.GetType(), value);
        }
    }

    public class Passed : Score
    {
        public Passed(bool value)
            : base(AttestationScoreType.Passed)
        {
            ScoreValue = new Tuple<Type, object>(value.GetType(), value);
        }
    }

    public class IsVisited : Score
    {
        public IsVisited(bool value)
            : base(AttestationScoreType.IsVisited)
        {
            ScoreValue = new Tuple<Type, object>(value.GetType(), value);
        }
    }

    public enum AttestationScoreType
    {
        NoResult,
        Five,
        Hundred,
        Passed,
        IsVisited,
    }
}
