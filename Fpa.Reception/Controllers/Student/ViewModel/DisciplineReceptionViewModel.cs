using Domain.Interface;
using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using reception.fitnesspro.ru.ViewModel;

namespace reception.fitnesspro.ru.Controllers.Student.ViewModel
{
    public class DisciplineReceptionViewModel
    {
        private readonly Domain.Reception reception;

        public List<string> CommonRejectReasons { get; set; } = new List<string>();
        public DateTime Date { get; set; }
        public List<PositionViewModel> Positions { get; set; } = new List<PositionViewModel>();

        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();

        public Guid ReceptionKey { get; set; }

        public DisciplineReceptionViewModel(Domain.Reception reception)
        {
            this.reception = reception;
            this.Date = reception.Date;
            ReceptionKey = reception.Key;
            this.Positions = reception.PositionManager.Positions
                //.Where(x => x.Record == default)
                .Select(x => new PositionViewModel
                {
                    Key = x.Key,
                    OffsetInMinutes = (int)(x.Time - Date.Date).TotalMinutes,
                    IsEmpty = x.Record == default,
                    Time = x.Time
                }).ToList();
            this.Events = reception.Events.Select(x => new EventViewModel(x)).ToList();
        }


        public void CheckContractExpired(Contract contract, IEnumerable<BaseConstraint> commonSettings)
        {
            if(contract == default) CommonRejectReasons.Add("The student`s contract was'nt found");

            // Срок договора не истек
            if (contract.IsContractExpiredForDay(reception.Date) == false) return;

            // Срок договора истек - проверяем переопределяют ли настройки дисциплин в рецепции правило - не проверять договор
            Events.ForEach(x => x.CheckContractExpired(contract, commonSettings, reception.Date));
        }

        public void CheckIsNotInPast()
        {
            if (reception.IsReceptionInPast()) CommonRejectReasons.Add("The date is in the past");
        }

        public void CheckEmptyPlaces()
        {
            if (reception.PositionManager.HasEmptyPlaces() == false) CommonRejectReasons.Add("There are not free places");
        }

        public void CheckSignUpBefore()
        {
            Events.ForEach(x => x.CheckSignUpBefore());
        }

        public void CheckAllowedDisciplinePeriod(Contract contract)
        {
            Events.ForEach(x => x.CheckAllowedDisciplinePeriod(contract, reception.Date));
        }

        public void CheckAttemptsCount(Guid disciplineKey, Guid studentKey, Contract contract, IStudentComponent logic)
        {
            Events.ForEach(async x => await x.CheckAttemptsCount(disciplineKey, studentKey, contract, logic));
        }

        public void CheckSignUpDoubles(Guid disciplineKey, Guid studentKey, IStudentComponent logic)
        {
            Events.ForEach(async x => await x.CheckSignUpDoubles(disciplineKey, studentKey, logic));
        }

        public void CheckDependencies(Guid disciplineKey, Guid studentKey, IReceptionComponent logic)
        {
            //Events.ForEach(async x => await x.CheckDependencies(disciplineKey, studentKey, logic));
        }

        public class PositionViewModel
        {
            public Guid Key { get; set; }
            public DateTime Time { get; set; }
            public bool IsEmpty { get; set; }
            public int OffsetInMinutes { get; set; }
        }

        public class EventViewModel
        {
            private Domain.Event @event;

            public EventViewModel(Domain.Event @event)
            {
                this.@event = @event;
                DisciplineKey = @event.Discipline.Key;
                Teachers = @event.Teachers;
            }

            public List<string> EventRejectReasons { get; set; } = new List<string>();

            public Guid DisciplineKey { get; set; }

            public IEnumerable<BaseInfo> Teachers { get; set; }

            public void CheckContractExpired(Contract contract, IEnumerable<BaseConstraint> commonSettings, DateTime date)
            {
                var programKey = contract.EducationProgram.Key;
                var groupKey = contract.Group.Key;
                var subGroupKey = contract.SubGroup.Key;

                var eventRule = @event.GetRestriction(programKey, groupKey, subGroupKey);

                if (eventRule != default && eventRule.Option.CheckContractExpired == false) return;

                // Если настроек на непроверку контракта в рецепии не существует пробуем проверить общие
                // if(commonSettings != default) ...
                //var commonRulesIsContractChecking = commonSettings.FirstOrDefault(
                //                            x=> x.DisciplineKey == @event.Discipline.Key &&
                //                            (x.ProgramKey == default || x.ProgramKey == programKey));
                // if(commonRulesIsContractChecking.CheckContract == false) return;

                if (contract.IsContractExpiredForDay(date)) EventRejectReasons.Add("The contract has expired");
            }

            public void CheckSignUpBefore()
            {
                if (@event.CanSignUpBefore(DateTime.Now) == false) EventRejectReasons.Add("The opportunity to register is in the past");
            }

            public void CheckAllowedDisciplinePeriod(Contract contract, DateTime date)
            {
                // get common limit
                var commonLimitDays = 5;

                var restriction = @event.GetRestriction(contract.EducationProgram.Key, contract.Group.Key, contract.SubGroup.Key);
                if (restriction == default || restriction.CheckAllowingPeriod() == false) return;

                if (contract.IsDateInPeriodFromStart(date, commonLimitDays)) EventRejectReasons.Add("The registration period for the discipline has expired");
            }

            public async Task CheckAttemptsCount(Guid disciplineKey, Guid studentKey, Contract contract, IStudentComponent logic)
            {
                // get common limit
                var commonLimitCount = 10;

                var restriction = @event.GetRestriction(contract.EducationProgram.Key, contract.Group.Key, contract.SubGroup.Key);
                if (restriction == default || restriction.CheckAttemps() == false) return;

                var receptions = await logic.GetReceptionsWithSignedUpStudent(studentKey);

                var filledByStudent = receptions.SelectMany(x => x.PositionManager.Positions)
                    .Where(x => x.Record != default && x.Record.StudentKey == studentKey && x.Record.DisciplineKey == disciplineKey)
                    .Where(x => x.Record.Result != default).ToList();

                if (filledByStudent.Count() >= commonLimitCount) EventRejectReasons.Add("The number of attempts for the discipline is over");
            }

            public async Task CheckSignUpDoubles(Guid disciplineKey, Guid studentKey, IStudentComponent logic)
            {
                var receptions = await logic.GetReceptionsWithSignedUpStudent(studentKey);

                var filledByStudent = receptions.SelectMany(x => x.PositionManager.Positions)
                    .Where(x => x.Record != default && x.Record.StudentKey == studentKey && x.Record.DisciplineKey == disciplineKey)
                    .Where(x => x.Record.Result == default).ToList();

                if (filledByStudent.Count() > 0) EventRejectReasons.Add("There is already one record on appropriate discipline");
            }

            public void CheckDependencies()
            {

            }

        }
    }
}
