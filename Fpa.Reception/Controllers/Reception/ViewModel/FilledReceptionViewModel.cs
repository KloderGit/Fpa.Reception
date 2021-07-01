using Domain;
using Mapster;
using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class FilledReceptionViewModel
    {
        private Domain.Reception reception;

        public FilledReceptionViewModel(Domain.Reception reception)
        {
            this.reception = reception;

            IsActive = reception.IsActive;
            Date = reception.Date;
            PositionManager = new PositionManagerViewModel(reception.PositionManager);
            Events = reception.Events.ToList().Select(x=> new EventViewModel(x)).ToList();
        }

        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public PositionManagerViewModel PositionManager { get; set; }
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();

        public IEnumerable<Guid> GetUsedProgramKeys()
        {
            var result = new List<Guid>();

            var recordProgramKeys = reception?.PositionManager?.Positions?.Select(x => x.Record?.ProgramKey).Where(x=>x.HasValue && x.Value != default).Select(x=>x.Value);
            if (recordProgramKeys != default) result.AddRange(recordProgramKeys);

            var payloadProgramKeys = reception?.Events?.SelectMany(x => x.Restrictions.Select(p => p.Program));
            if (payloadProgramKeys != default) result.AddRange(payloadProgramKeys);

            return result.Distinct();
        }

        public IEnumerable<Guid> GetUsedDisciplineKeys()
        {
            var result = new List<Guid>();

            var recordDisciplineKeys = reception?.PositionManager?.Positions?.Select(x => x.Record?.DisciplineKey).Where(x=>x.HasValue && x.Value != default).Select(x=>x.Value);
            if (recordDisciplineKeys != default) result.AddRange(recordDisciplineKeys);

            var requirementDisciplineKeys = reception?.Events?.SelectMany(x => x.Requirement.DependsOnOtherDisciplines).Where(x=>x != default);
            if (requirementDisciplineKeys != default) result.AddRange(requirementDisciplineKeys);

            return result.Distinct();
        }

        public IEnumerable<Guid> GetUsedGroupKeys()
        {
            var result = new List<Guid>();

            var payloadGroupKeys = reception?.Events?.SelectMany(x => x.Restrictions.Select(p => p?.Group)).Where(x=>x.HasValue && x.Value != default).Select(x=>x.Value);
            if (payloadGroupKeys != default) result.AddRange(payloadGroupKeys);

            return result.Distinct();
        }

        public IEnumerable<Guid> GetUsedSubGroupKeys()
        {
            var result = new List<Guid>();

            var payloadSubGroupKeys = reception?.Events?.SelectMany(x => x.Restrictions.Select(p => p?.SubGroup)).Where(x=>x.HasValue && x.Value != default).Select(x=>x.Value);
            if (payloadSubGroupKeys != default) result.AddRange(payloadSubGroupKeys);

            return result.Distinct();
        }

        public IEnumerable<Guid> GetUsedStudentKeys()
        {
            var result = new List<Guid>();

            var recordStudentKeys = reception?.PositionManager?.Positions?.Select(x => x.Record?.StudentKey).Where(x=>x.HasValue && x.Value != default).Select(x=>x.Value);
            if (recordStudentKeys != default) result.AddRange(recordStudentKeys);

            return result.Distinct();
        }

        public IEnumerable<Guid> GetUsedTeacherKeys()
        {
            var result = new List<Guid>();

            var recordTeacherKeys = reception?.PositionManager?.Positions?.Select(x => x.Record?.Result?.TeacherKey).Where(x=>x.HasValue && x.Value != default).Select(x=>x.Value);
            if (recordTeacherKeys != default) result.AddRange(recordTeacherKeys);

            return result.Distinct();
        }

        public IEnumerable<Guid> GetUsedRateKeys()
        {
            var result = new List<Guid>();

            var recordTeacherKeys = reception?.PositionManager?.Positions?.Select(x => x.Record?.Result?.RateKey).Where(x=>x.HasValue && x.Value != default).Select(x=>x.Value);
            if (recordTeacherKeys != default) result.AddRange(recordTeacherKeys);

            return result.Distinct();
        }

        public class PositionManagerViewModel
        {
            public PositionManagerViewModel(PositionManager positionManager)
            {
                if(positionManager == default) return;

                LimitType = (PositionTypeViewModel)(int)positionManager.LimitType;
                Positions = positionManager.Positions.Select(x => new PositionViewModel(x));
            }

            public PositionTypeViewModel LimitType;
            public IEnumerable<PositionViewModel> Positions { get; set; } = new List<PositionViewModel>();

            public class PositionViewModel
            {
                public PositionViewModel(Position position)
                {
                    if(position == default) return;

                    Key = position.Key;
                    IsActive = position.IsActive;
                    Time = position.Time;
                    Record = new RecordViewModel(position.Record);
                }
                public Guid Key { get; set; }
                public bool IsActive { get; set; }
                public DateTime Time { get; set; }
                public RecordViewModel Record { get; set; }
            }

            public class RecordViewModel
            {
                public RecordViewModel(Record record)
                {
                    if(record == default) return;

                    Student = record.StudentKey == default ? null : new BaseInfoViewModel { Key = record.StudentKey };
                    Program = record.ProgramKey == default ? null : new BaseInfoViewModel { Key = record.ProgramKey };
                    Discipline = record.DisciplineKey == default ? null :  new BaseInfoViewModel { Key = record.DisciplineKey };
                    Result = new ResultViewMOdel(record.Result);
                }
                public BaseInfoViewModel Student { get; set; }
                public BaseInfoViewModel Program { get; set; }
                public BaseInfoViewModel Discipline { get; set; }
                public ResultViewMOdel Result { get; set; }
            }

            public class ResultViewMOdel
            {
                public ResultViewMOdel(Result result)
                {
                    if(result == default) return;

                    Teacher =  result.TeacherKey == default ? null : new BaseInfoViewModel { Key = result.TeacherKey };
                    Rate = result.RateKey == default ? null : new BaseInfoViewModel { Key = result.RateKey };
                    Comment = result.Comment;
                }
                public BaseInfoViewModel Teacher { get; set; }
                public BaseInfoViewModel Rate { get; set; }
                public string Comment { get; set; }
            }

            public enum PositionTypeViewModel
            {
                Seating,
                Number,
                Free
            }
        }

        public class EventViewModel
        {
            public EventViewModel(Event @event)
            {
                if(@event == default) return;

                Teachers = @event.Teachers?.Adapt<IEnumerable<BaseInfoViewModel>>().ToList();
                Discipline = @event.Discipline?.Adapt<BaseInfoViewModel>();
                Restrictions = @event.Restrictions.Select(x=> new PayloadRestrictionViewModel(x)).ToList();
                Requirement = new PayloadRequirementViewModel(@event.Requirement);
            }

            public List<BaseInfoViewModel> Teachers { get; set; } = new List<BaseInfoViewModel>();
            public BaseInfoViewModel Discipline { get; set; }

            public List<PayloadRestrictionViewModel> Restrictions { get; set; } = new List<PayloadRestrictionViewModel>();
            public PayloadRequirementViewModel Requirement { get; set; }

            public class PayloadRestrictionViewModel
            {
                public PayloadRestrictionViewModel(PayloadRestriction payloadRestriction)
                {
                    if(payloadRestriction == default) return;

                    Program =  payloadRestriction.Program == default ? null : new BaseInfoViewModel{ Key = payloadRestriction.Program };
                    Group = payloadRestriction.Group == default ? null : new BaseInfoViewModel{ Key = payloadRestriction.Group };
                    SubGroup = payloadRestriction.SubGroup == default ? null :new BaseInfoViewModel{ Key = payloadRestriction.SubGroup };
                    Option = new PayloadOptionViewModel(payloadRestriction.Option);
                }
                public BaseInfoViewModel Program { get; set; }
                public BaseInfoViewModel Group { get; set; }
                public BaseInfoViewModel SubGroup { get; set; }
                public PayloadOptionViewModel Option { get; set; }
            }

            public class PayloadOptionViewModel
            {
                public PayloadOptionViewModel(PayloadOption option)
                {
                    if(option == default) return;

                    CheckContractExpired = option.CheckContractExpired;
                    CheckDependings = option.CheckDependings;
                    CheckAttemps = option.CheckAttemps;
                    CheckAllowingPeriod = option.CheckAllowingPeriod;
                }
                public bool CheckContractExpired { get; set; } = true;
                public bool CheckDependings { get; set; } = true;
                public bool CheckAttemps { get; set; } = true;
                public bool CheckAllowingPeriod { get; set; } = true;
            }

            public class PayloadRequirementViewModel
            {
                public PayloadRequirementViewModel(PayloadRequirement payloadRequirement)
                {
                    if(payloadRequirement == default) return;

                    SubscribeBefore = payloadRequirement.SubscribeBefore;
                    UnsubscribeBefore = payloadRequirement.UnsubscribeBefore;
                    AllowedAttemptCount = payloadRequirement.AllowedAttemptCount;
                    DependsOnOtherDisciplines = payloadRequirement.DependsOnOtherDisciplines.Where(x=>x != default).Select(x=> new BaseInfoViewModel{ Key = x }).ToList();
                }
                public DateTime SubscribeBefore { get; set; } = default;
                public DateTime UnsubscribeBefore { get; set; } = default;

                public List<BaseInfoViewModel> DependsOnOtherDisciplines { get; set; } = new List<BaseInfoViewModel>();
                public int AllowedAttemptCount { get; set; }
            }
        }
    }
}
