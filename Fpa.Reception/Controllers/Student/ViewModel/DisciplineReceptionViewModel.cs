using Domain.Interface;
using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student.ViewModel
{
    public class DisciplineReceptionViewModel
    {
        private readonly Domain.Reception reception;

        public List<string> CommonRejectReasons { get; set; } = new List<string>();
        public DateTimeOffset Date { get; set; }
        public List<PositionViewModel> Positions { get; set; } = new List<PositionViewModel>();

        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();


        public DisciplineReceptionViewModel(Domain.Reception reception)
        {
            this.reception = reception;
            this.Date = reception.Date;
            this.Positions = reception.PositionManager.Positions
                .Where(x => x.Record == default)
                .Select(x => new PositionViewModel { Key = x.Key, Time = x.Time - Date.Date }).ToList();
            this.Events = reception.Events.Select(x => new EventViewModel(x)).ToList();
        }


        public void CheckContractExpired(Contract contract)
        {
            if (contract.ExpiredDate != default && contract.ExpiredDate < DateTime.Now) CommonRejectReasons.Add("The contract has expired");
        }

        public void CheckIsNotInPast()
        {
            if (Date < DateTime.Now) CommonRejectReasons.Add("The date is in the past");
        }

        public void CheckEmptyPlaces()
        {
            if (Positions == default || Positions.Any() == false) CommonRejectReasons.Add("There are not free places");
        }

        public void CheckEvents(Guid disciplineKey, Guid studentKey, Contract contract, IReceptionComponent logic)
        {
            Events.ForEach(x => x.CheckSignUpBefore());
            Events.ForEach(x => x.CheckAllowedDisciplinePeriod(contract));
            Events.ForEach(x => x.CheckAttemptsCount(disciplineKey, studentKey, contract, logic));
            Events.ForEach(x => x.CheckSignUpDoubles(disciplineKey, studentKey, logic));
            //Events.ForEach(x => x.CheckDependencies());
        }

        public class PositionViewModel
        {
            public Guid Key { get; set; }
            public TimeSpan Time { get; set; }
        }

        public class EventViewModel
        {
            private Domain.Event @event;

            public EventViewModel(Domain.Event @event)
            {
                this.@event = @event;
            }

            public List<string> EventRejectReasons { get; set; } = new List<string>();

            public Guid DisciplineKey { get; set; }


            public void CheckSignUpBefore()
            {
                if (@event?.Requirement?.SubscribeBefore == default) return;
                if (@event.Requirement.SubscribeBefore > DateTime.Now) EventRejectReasons.Add("The opportunity to register is in the past");
            }

            public void CheckAllowedDisciplinePeriod(Contract contract)
            {
                // get common limit
                var commonLimitDays = 180;

                var restrictions = @event.Restrictions.Where(x=>x.Option != default && x.Option.CheckAllowingPeriod != default)
                    .Where(x => x.Program == contract.EducationProgram.Key || x.Program == default)
                    .Where(x => x.Group == contract.Group.Key || x.Group == default)
                    .Where(x => x.SubGroup == contract.SubGroup.Key || x.SubGroup == default);

                if (restrictions == default) return;

                var overridedChekingValue = restrictions.FirstOrDefault().Option.CheckAllowingPeriod;

                if (overridedChekingValue == false) return;

                if (contract.StartEducationDate.AddDays(commonLimitDays) > DateTime.Now) EventRejectReasons.Add("The registration period for the discipline has expired");
            }

            public async Task CheckAttemptsCount(Guid disciplineKey, Guid studentKey, Contract contract, IReceptionComponent logic)
            {
                // get common limit
                var commonLimitCount = 10;

                var restrictions = @event.Restrictions.Where(x => x.Option != default && x.Option.CheckAttemps != default)
                    .Where(x => x.Program == contract.EducationProgram.Key || x.Program == default)
                    .Where(x => x.Group == contract.Group.Key || x.Group == default)
                    .Where(x => x.SubGroup == contract.SubGroup.Key || x.SubGroup == default);

                if (restrictions == default) return;

                var overridedChekingValue = restrictions.FirstOrDefault().Option.CheckAttemps;

                if (overridedChekingValue == false) return;

                var receptions = await logic.GetByStudentKey(studentKey);

                var filledByStudent = receptions.SelectMany(x => x.PositionManager.Positions)
                    .Where(x => x.Record != default && x.Record.StudentKey == studentKey && x.Record.DisciplineKey == disciplineKey)
                    .Where(x => x.Record.Result != default).ToList();

                if (filledByStudent.Count() >= commonLimitCount) EventRejectReasons.Add("The number of attempts for the discipline is over");
            }

            public async Task CheckSignUpDoubles(Guid disciplineKey, Guid studentKey, IReceptionComponent logic)
            {
                var receptions = await logic.GetByStudentKey(studentKey);

                var filledByStudent = receptions.SelectMany(x => x.PositionManager.Positions)
                    .Where(x => x.Record != default && x.Record.StudentKey == studentKey && x.Record.DisciplineKey == disciplineKey)
                    .Where(x => x.Record.Result == default).ToList();

                if (filledByStudent.Count() > 0) EventRejectReasons.Add("There is alredy one record on appropriate discipline");
            }

            public void CheckDependencies()
            {
                
            }

        }
    }
}
