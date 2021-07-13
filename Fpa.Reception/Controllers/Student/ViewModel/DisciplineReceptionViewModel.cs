using Domain.Interface;
using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using reception.fitnesspro.ru.ViewModel;
using Domain.Model;
using Application.Extensions;

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
            if (contract == default) CommonRejectReasons.Add("The student`s contract was'nt found");

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

        public void CheckAllowedDisciplinePeriod(IEnumerable<Domain.GroupSettings.EventPeriodConstraint> settings, Contract contract)
        {
            Events.ForEach(x => x.CheckAllowedDisciplinePeriod(settings, contract, reception.Date));
        }

        public void CheckAttemptsCount(Guid disciplineKey, Contract contract, StudentSetting studentSetting)
        {
            Events.ForEach(x => x.CheckAttemptsCount(disciplineKey, contract, studentSetting));
        }

        public void CheckSignUpDoubles(Guid disciplineKey, Guid studentKey, IStudentComponent logic)
        {
            Events.ForEach(async x => await x.CheckSignUpDoubles(disciplineKey, studentKey, logic));
        }

        public void CheckDependencies(Guid disciplineKey, Contract contract, IEnumerable<BaseConstraint> constraints, IEnumerable<Position> positions)
        {
            Events.ForEach(x => x.CheckDependencies(disciplineKey, contract, constraints, positions));
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

            public void CheckAllowedDisciplinePeriod(IEnumerable<Domain.GroupSettings.EventPeriodConstraint> settings, Contract contract, DateTime date)
            {
                if (contract == default || (contract.StartEducationDate == default && contract.FinishEducationhDate == default))
                    EventRejectReasons.Add("The student contract was not defined or the term of study was specified incorrectly");

                var checkAllowingPeriod = true;

                var restriction = @event.GetRestriction(contract.EducationProgram.Key, contract.Group.Key, contract.SubGroup.Key);
                if (restriction != default && restriction.CheckAllowingPeriod() == false) checkAllowingPeriod = false;

                if (checkAllowingPeriod == false) return;

                var disciplineSetting = settings.FirstOrDefault(x => x.Discipline.Key == this.DisciplineKey);

                if (disciplineSetting == default)
                {
                    if (contract.IsContractExpiredForDay(date)) EventRejectReasons.Add("The registration period for the discipline has expired");
                    return;
                }

                var startPeriodDate = disciplineSetting.StartPeriod == default ? contract.StartEducationDate : disciplineSetting.StartPeriod;
                var finishPeriodDate = disciplineSetting.FinishPeriod == default ? contract.FinishEducationhDate : disciplineSetting.FinishPeriod;

                if (startPeriodDate < finishPeriodDate) SwapDate(ref startPeriodDate, ref finishPeriodDate);

                if ((date > startPeriodDate && date < finishPeriodDate) == false) EventRejectReasons.Add("The registration period for the discipline has expired");

                void SwapDate(ref DateTime start, ref DateTime finish)
                {
                    var tmp = finish;
                    finish = start;
                    start = tmp;
                }
            }

            public void CheckAttemptsCount(Guid disciplineKey, Contract contract, StudentSetting studentSetting)
            {
                var restriction = @event.GetRestriction(contract.EducationProgram.Key, contract.Group.Key, contract.SubGroup.Key);
                if (restriction == default || restriction.CheckAttemps() == false) return;

                if (studentSetting != default && studentSetting.IsDisciplineSettingExists(disciplineKey))
                {
                    var signUpCount = studentSetting.GetRestSignUpCount(disciplineKey);
                    if (signUpCount.HasValue)
                    {
                        if (signUpCount.Value <= 0) EventRejectReasons.Add("The number of attempts for the discipline is over");
                    }
                }
            }

            public async Task CheckSignUpDoubles(Guid disciplineKey, Guid studentKey, IStudentComponent logic)
            {
                var receptions = await logic.GetReceptionsWithSignedUpStudent(studentKey);

                var filledByStudent = receptions.SelectMany(x => x.PositionManager.Positions)
                    .Where(x => x.Record != default && x.Record.StudentKey == studentKey && x.Record.DisciplineKey == disciplineKey)
                    .Where(x => x.Record.Result == default).ToList();

                if (filledByStudent.Count() > 0) EventRejectReasons.Add("There is already one record on appropriate discipline");
            }

            public void CheckDependencies(Guid disciplineKey, Contract contract, IEnumerable<BaseConstraint> constraints, IEnumerable<Position> positions)
            {
                var restriction = @event.GetRestriction(contract.EducationProgram.Key, contract.Group.Key, contract.SubGroup.Key);
                if (restriction != default && restriction.CheckDependings() == false) return;

                if (@event.Requirement == default || @event.Requirement.DependsOnOtherDisciplines.IsNullOrEmpty()) return;

                var dependings = @event.Requirement.DependsOnOtherDisciplines;

                //if (contract == default || contract.EducationProgram == default || contract.EducationProgram.Key == default)
                //    EventRejectReasons.Add("The student contract was not defined or the education program was'nt specified");

                //var constraint = constraints.FirstOrDefault(x => x.ProgramKey == contract.EducationProgram.Key);
                //if (constraint == default) constraint = constraints.FirstOrDefault();

                //if (constraint != default) dependings = constraint.DependsOn?.Select(x => x.Key);

                //if (dependings == default) return;

                var sucsesfullRates = new List<Guid>()
                {
                    new Guid("6367ef35-ed62-4b18-981e-10f749f5caeb"), // Зачтено
                    new Guid("fb0a2324-061d-11e6-ab08-c8600054f636"), // Отлично
                    new Guid("fb0a2325-061d-11e6-ab08-c8600054f636"), // Хорошо
                    new Guid("fb0a2326-061d-11e6-ab08-c8600054f636"), // Удовл.
                };

                var gottedRates = positions
                    .Where(x => x.Record != default)
                    .Where(x => dependings.Contains(x.Record.DisciplineKey))
                    .Where(x => x.Record.Result != default)
                    .Where(x => x.Record.Result.RateKey != default)
                    .Select(x => x.Record.Result.RateKey);

                var result = sucsesfullRates.Intersect(gottedRates);

                if (result.Count() == 0) EventRejectReasons.Add("That discipline is depended on other discipline results");
            }

        }
    }
}
