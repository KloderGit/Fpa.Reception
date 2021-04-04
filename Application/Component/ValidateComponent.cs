using Application.Extensions;
using Domain;
using Domain.Interface;
using Domain.Model.Education;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.MongoDB;

namespace Application.Component
{
    public class ValidateComponent : IValidateComponent
    {
        private readonly MongoContext database;
        private readonly Context lcService;

        public ValidateComponent(MongoContext mongo, Context lcService)
        {
            this.database = mongo;
            this.lcService = lcService;
        }

        public async Task<bool> CheckContractExpired(Contract contract)
        {
            return contract.ExpiredDate != default && contract.ExpiredDate < DateTime.Now;
        }

        public async Task<bool> CheckEmptyPlaces(Reception reception)
        {
            if (reception.PositionManager.LimitType == PositionType.Free) return true;

            if (reception.PositionManager.Positions.Select(x => x.Record != default).Count() < reception.PositionManager.Positions.Count()) return true;

            return false;
        }

        public bool CheckIsNotInPast(DateTime date)
        {
            if (date < DateTime.Now) return true;

            return false;
        }



        public async Task<bool> CheckAttemptsCount(Event @event, Guid studentKey)
        {
            var allStudentReceptions = database.Receptions.Repository.FilterByPath("PositionManager.Positions.Record.StudentKey", studentKey).ToList();

            var receptionsByDiscipline = allStudentReceptions.SelectMany(x => x.Events).Select(x => x.Discipline).Where(x=>x.Key == @event.Discipline.Key);

            var commonDisciplineCountLimit = 10; // constrintComponent 

            if (receptionsByDiscipline.Count() < commonDisciplineCountLimit) return true;

            return false;
        }

        public async Task<bool> CheckSignUpDoubles(Guid disciplineKey, Guid studentKey)
        {
            var allStudentReceptions = database.Receptions.Repository.FilterByPath("PositionManager.Positions.Record.StudentKey", studentKey).ToList();

            var positions = allStudentReceptions
                .SelectMany(x => x.PositionManager.Positions)
                .Where(x => x.Record != default && x.Record.DisciplineKey == disciplineKey)
                .Where(x=>x.Record.Result != default && x.Record.Result.Comment != null);

            if (positions.Count() > 0) return false;

            return true;
        }

        public async Task<bool> CheckSignUpBefore(Event @event)
        {
            if (@event.Requirement.SubscribeBefore > DateTime.Now) return false;

            return true;
        }

        public async Task<bool> CheckAllowedDisciplinePeriod(Event @event, Contract contract)
        {
            var commonDisciplinePeriodLimit = 160; // constrintComponent 

            var restriction = GetRestriction(@event, contract);

            var toCheckPeriod = restriction?.Option.CheckAllowingPeriod;

            if (toCheckPeriod == false) return true;

            var dayspan = (DateTime.Now.Date - contract.StartEducationDate).Days;

            if (dayspan < commonDisciplinePeriodLimit) return true;

            return false;
        }


        private PayloadRestriction GetRestriction(Event @event, Contract contract)
        {
            var programKey = contract.EducationProgram.Key;
            var groupKey = contract.Group.Key;
            var subGroupKey = contract.SubGroup.Key;

            var resrtictions = @event.Restrictions?.Where(x => x.Program == default);
            if (resrtictions.Count() == 1) return resrtictions.FirstOrDefault();

            resrtictions = @event.Restrictions?.Where(x => x.Program == programKey);
            if (resrtictions.Count() == 1) return @event.Restrictions.FirstOrDefault();

            resrtictions = resrtictions.Where(x => x.Group == groupKey);
            if (resrtictions.Count() == 1) return @event.Restrictions.FirstOrDefault();

            resrtictions = resrtictions.Where(x => x.SubGroup == subGroupKey);
            if (resrtictions.Count() == 1) return @event.Restrictions.FirstOrDefault();

            return null;
        }

        //public async Task<bool> CheckDependencies(Event @event, Guid studentKey)
        //{
        //    var dependencies = new Guid[] { Guid.NewGuid() }; // From common limit
        //    dependencies = new Guid[] { Guid.NewGuid() };

        //    var receptions = database.Receptions.FilterByPath("PositionManager.Positions.Record.StudentKey", studentKey)
        //        .ToList()
        //        .Adapt<List<Reception>>();

        //    var depended = receptions.Where(x => x.Events.Select(e=>e.Discipline.Key).Any(e=> dependencies.Contains(e)));

        //    var resulted = depended.Select(x => x.PositionManager.GetSignedUpStudentPosition(studentKey).FirstOrDefault()).Select(x => new { Discipline = x.Record.DisciplineKey, Result = x.Record.Result.Score.Value });

        //    return true;
        //}
    }
}
