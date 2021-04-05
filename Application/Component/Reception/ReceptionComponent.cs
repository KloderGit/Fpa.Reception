using Domain;
using Domain.Interface;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.MongoDB;

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
            var dto = database.Receptions.Repository.AsQueryable().ToList();

            var result = dto.Adapt<List<Reception>>();

            return result;
        }

        public Reception Get(Guid key)
        {
            var dto = database.Receptions.Repository.FindOne(x => x.Key == key);

            var result = dto.Adapt<Reception>();

            return result;
        }

        public void Update(Reception reception)
        {
            var dto = reception.Adapt<Service.MongoDB.Model.Reception>();
                
            database.Receptions.Repository.ReplaceOne(dto);
        }

        public async Task<IEnumerable<Reception>> GetByDisciplineKey(Guid disciplineKey)
        {
            var dto = await database.Receptions.GetByDiscipline(disciplineKey);

            return dto.Adapt<List<Reception>>();
        }

        public async Task<IEnumerable<Reception>> GetByPosition(Guid positionKey)
        {
            var dto = await database.Receptions.GetByPosition(positionKey);

            return dto.Adapt<List<Reception>>();
        }

        public async Task<IEnumerable<Reception>> GetByStudentKey(Guid studentKey)
        {
            var dto = await database.Receptions.GetByStudent(studentKey);

            return dto.Adapt<List<Reception>>();
        }

        public async Task<IEnumerable<Reception>> GetByTeacherKey(Guid teacherKey)
        {
            var dto = await database.Receptions.GetByTeacher(teacherKey);

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

            var dtoReceptions = database.Receptions.Repository.FilterByArray("Events.Discipline.Key", disciplineKeys).ToList(); // and Date > 1 date of this month
            var receptions = dtoReceptions.Adapt<List<Reception>>();

            return receptions;
        }

        public void Store(Reception reception)
        {
            var dto = reception.ConvertToType<Service.MongoDB.Model.Reception>(ReceptionConverter.ConvertToMongoDto);

            database.Receptions.Repository.InsertOne(dto);
        }



        public IEnumerable<Reception> GetByDisciplineKeys(IEnumerable<Guid> keys)
        {
            var dto = database.Receptions.Repository.FilterByArray("Events.Discipline.Key", keys).ToList();

            return dto.Adapt<List<Reception>>();
        }

        public async Task ReplaceReception(Reception reception)
        {
            var serviceReception = await database.Receptions.GetByKeyAsync(reception.Key);
            
            var dto = reception.Adapt(serviceReception);

            database.Receptions.Repository.ReplaceOne(dto);
        }
    }
}
