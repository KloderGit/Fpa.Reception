using Domain;
using Domain.Interface;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Component
{
    public class ReceptionComponent : IReceptionComponent
    {
        private readonly MongoContext database;
        private readonly Context lcContext;

        public ReceptionComponent(MongoContext database, Context lcContext)
        {
            this.database = database;
            this.lcContext = lcContext;
        }

        public IEnumerable<Reception> Get()
        {
            var dto = database.Receptions.AsQueryable().ToList();

            var result = dto.Adapt<List<Reception>>();

            return result;
        }

        public Reception Get(Guid key)
        {
            var dto = database.Receptions.FindOne(x => x.Key == key);

            var result = dto.Adapt<Reception>();

            return result;
        }

        public void ReplaceReception(Reception reception)
        {
            var dto = reception.Adapt<Service.MongoDB.Model.Reception>();
                
            database.Receptions.ReplaceOne(dto);
        }

        public IEnumerable<Reception> GetByDisciplineKey(Guid key)
        {
            var dto = database.Receptions.FilterByPath("Events.Discipline.Key", key).ToList();

            return dto.Adapt<List<Reception>>();
        }

        public IEnumerable<Reception> GetByPosition(Guid key)
        {
            var dto = database.Receptions.FilterByPath("PositionManager.Positions.Key", key).ToList();

            return dto.Adapt<List<Reception>>();
        }

        public IEnumerable<Reception> GetByStudentKey(Guid studentKey)
        {
            var dto = database.Receptions.FilterByPath("PositionManager.Positions.Record.StudentKey", studentKey);

            return dto.Adapt<List<Reception>>();
        }

        public IEnumerable<Reception> GetByTeacherKey(Guid key)
        {
            var dto = database.Receptions.FilterByPath("Events.Teachers.Key", key);

            return dto.Adapt<List<Reception>>();
        }

        public async Task<dynamic> GetReceptions(Guid studentKey, Guid programKey)
        {
            var contractManager = lcContext.Contract;
            var contractsByProgram = await contractManager.FindForStudentByProgram(studentKey, programKey);

            var contract = contractsByProgram
                .Where(x=>x.ExpiredDate > DateTime.Now.Date)
                .FirstOrDefault(x => x.ExpiredDate == contractsByProgram.Max(d => d.ExpiredDate));

            var conteractProgramKey = contract.EducationProgram.Key;
            var conteractgroupKey = contract.Group.Key;
            var conteractsubGroupKey = contract.SubGroup.Key;

            var programManager = lcContext.Program;
            var program = await programManager.GetProgram(conteractProgramKey);

            var disciplines = program.Educations.Where(x => x.ControlType.Key != default).Select(x=>x.Discipline.Key);

            var receptions = GetByDisciplineKeys(disciplines);

            //var result = receptions.Where(x => x.Events.Where(e => e.Restrictions.Where(r => r.Program == program || r.Program == default)))
            //                       .Where(x => x.Events.Where(e => e.Restrictions.Where(r => r.Group == group || r.Group == default)))
            //                       .Where(x => x.Events.Where(e => e.Restrictions.Where(r => r.SubGroup == subGroup || r.SubGroup == default)));

            return receptions;
        }

        public async Task<IEnumerable<Reception>> GetProgramReceptions(Guid programKey)
        {
            var program = await lcContext.Program.GetProgram(programKey);
            var disciplineKeys = program.Educations.Select(x => x.Discipline.Key).ToList();

            var dtoReceptions = database.Receptions.FilterByArray("Events.Discipline.Key", disciplineKeys).ToList(); // and Date > 1 date of this month
            var receptions = dtoReceptions.Adapt<List<Reception>>();

            return receptions;
        }

        public void Store(Reception reception)
        {
            var dto = reception.ConvertToType<Service.MongoDB.Model.Reception>(ReceptionConverter.ConvertToMongoDto);

            database.Receptions.InsertOne(dto);
        }

        public IEnumerable<Reception> GetByDisciplineKeys(IEnumerable<Guid> keys)
        {
            var dto = database.Receptions.FilterByArray("Events.Discipline.Key", keys).ToList();

            return dto.Adapt<List<Reception>>();
        }
    }
}
