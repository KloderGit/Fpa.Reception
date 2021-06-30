using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Reception : KeyEntity
    {
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public PositionManager PositionManager { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();

        public List<History> Histories { get; set; } = new List<History>();


        public bool IsForProgram(Guid programKey)
        {
            var result = Events.Where(x => x.Restrictions.Any() == false || x.Restrictions.Any(p => p.Program.Key == programKey || p.Program == default)).ToList();

            return result != default && result.Any();
        }

        public bool IsForGroup(Guid groupKey)
        {
            var result = Events.Where(x => x.Restrictions.Any() == false || x.Restrictions.Any(p => p.Group.Key == groupKey || p.Group == default));

            return result != default && result.Any();
        }

        public bool IsForSubGroup(Guid subGroupKey)
        {
            var result = Events.Where(x => x.Restrictions.Any() == false || x.Restrictions.Any(p => p.SubGroup.Key == subGroupKey || p.SubGroup == default));

            return result != default && result.Any();
        }


        public bool IsReceptionInPast()
        {
            if (Date < DateTime.Now) return true;
            return false;
        }

        public bool HasEmptyPlaces()
        {
            return PositionManager.HasEmptyPlaces();
        }

        public void ClearRecords()
        {
            PositionManager.Positions.ToList().ForEach(x => x.Record = null);
        }

        public void ChangeData(DateTime date)
        {
            this.Date = date.Date;
            PositionManager.Positions.ToList().ForEach(x => x.Time = MakePositionTime(x.Time));

            DateTime MakePositionTime(DateTime dateTime)
            {
                var newDateTime = Date.AddHours(dateTime.Hour).AddMinutes(dateTime.Minute);
                return newDateTime;
            }
        }

        public TConverted ConvertToType<TConverted>(Func<Reception, TConverted> function)
        {
            var result = function(this);

            return result;
        }

        public Reception ConvertFromType<TConverted>(Func<TConverted, Reception> function, TConverted item)
        {
            var result = function(item);

            return result;
        }
    }

    public class PositionManager
    {
        public PositionType LimitType;
        public PositionManager()
        {
        }
        public List<Position> Positions { get; set; } = new List<Position>();

        public bool HasEmptyPlaces()
        {
            if (LimitType == PositionType.Free) return true;
            if (Positions == default || Positions.Any() == false) return false;

            var positions = Positions.Where(x => x.Record == default || x.Record.StudentKey == default);

            if (positions == default || positions.Any() == false) return false;

            return true;
        }

        public Position GetPositionByKey(Guid positionKey)
        {
            var position = Positions.FirstOrDefault(x => x.Key == positionKey);

            return position;
        }

        public IEnumerable<Position> GetSignedUpStudentPosition(Guid studentKey)
        {
            var positions = Positions.Where(x => x.Record != default && x.Record.StudentKey == studentKey);

            return positions;
        }
    }

    public enum PositionType
    {
        Seating,
        Number,
        Free
    }


    public class Event
    {
        public List<BaseInfo> Teachers { get; set; } = new List<BaseInfo>();
        public BaseInfo Discipline { get; set; }
        public List<PayloadRestriction> Restrictions { get; set; } = new List<PayloadRestriction>();
        public PayloadRequirement Requirement { get; set; }

        public bool CanSignUpBefore(DateTime date)
        {
            if (Requirement == default) return true;
            if (Requirement.SubscribeBefore == default) return true;
            if (date > Requirement.SubscribeBefore) return false;

            return true;
        }

        public PayloadRestriction GetRestriction(Guid programKey, Guid groupKey, Guid subgroupKey)
        {
            if (Restrictions == default) return null;

            var restriction = Restrictions
                .Where(x => x.Program.Key == programKey || x.Program == default)
                .Where(x => x.Group.Key == groupKey || x.Group == default)
                .Where(x => x.SubGroup.Key == subgroupKey || x.SubGroup == default)
                .FirstOrDefault();

            return restriction;
        }
    }

    public class PayloadRequirement
    {
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;

        public IEnumerable<BaseInfo> DependsOnOtherDisciplines { get; set; } = new List<BaseInfo>();
        public int AllowedAttemptCount { get; set; }
    }

    public class PayloadRestriction
    {
        public BaseInfo Program { get; set; }
        public BaseInfo Group { get; set; }
        public BaseInfo SubGroup { get; set; }

        public PayloadOption Option { get; set; }

        public bool CheckContractExpired()
        {
            if (Option != default && Option.CheckContractExpired == false) return false;
            return true;
        }

        public bool CheckDependings()
        {
            if (Option != default && Option.CheckDependings == false) return false;
            return true;
        }

        public bool CheckAttemps()
        {
            if (Option != default && Option.CheckAttemps == false) return false;
            return true;
        }

        public bool CheckAllowingPeriod()
        {
            if (Option != default && Option.CheckAllowingPeriod == false) return false;
            return true;
        }
    }

    public class PayloadOption
    {
        public bool CheckContractExpired { get; set; } = true;
        public bool CheckDependings { get; set; } = true;
        public bool CheckAttemps { get; set; } = true;
        public bool CheckAllowingPeriod { get; set; } = true;
    }

}
