using Domain;
using Domain.Education;
using Domain.Interface;
using Domain.Model.Education;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.MongoDB;

namespace Application.Component
{
    public class StudentComponent : IStudentComponent
    {
        private readonly Context lcservice;
        private readonly MongoContext database;

        public StudentComponent(MongoContext mongo, Context lcservice)
        {
            this.lcservice = lcservice;
            this.database = mongo;
        }

        public async Task<IEnumerable<Reception>> GetAttestation(Guid studentKey, Guid programKey)
        {
            //Найти договор студента по программе

            var contractManager = lcservice.Contract;
            var contractsByProgram = await contractManager.FindForStudentByProgram(studentKey, programKey);

            var contract = contractsByProgram
                .Where(x => x.ExpiredDate > DateTime.Now.Date)
                .FirstOrDefault(x => x.ExpiredDate == contractsByProgram.Max(d => d.ExpiredDate));

            //Получить полные данные о обучении студента Прогрмма \ Группа \ Подгруппа

            var contractProgramKey = contract.EducationProgram.Key;
            var contractgroupKey = contract.Group.Key;
            var contractsubGroupKey = contract.SubGroup.Key;

            // Получить все экзамены по программе

            var programManager = lcservice.Program;
            var program = await programManager.GetProgram(contractProgramKey);

            var disciplines = program.Educations.Where(x => x.ControlType.Key != default).Select(x => x.Discipline.Key);

            var dto = database.Receptions.Repository.FilterByArray("Events.Discipline.Key", disciplines).ToList();

            var domen = dto.Adapt<List<Reception>>();

            var result = domen.Where(x => x.IsForProgram(contractProgramKey));
            result = result.Where(x => x.IsForGroup(contractgroupKey));
            result = result.Where(x => x.IsForSubGroup(contractgroupKey));

            return result;
        }


        public async Task<IEnumerable<Contract>> GetContracts(Guid studentKey)
        {
            var dto = await lcservice.Contract.GetByStudent(studentKey);

            var result = dto.Adapt<IEnumerable<Contract>>();

            return result;
        }



        public async Task<IEnumerable<Reception>> GetAppointments(Guid studentKey)
        {
            var dto = database.Receptions.Repository.FilterByPath("PositionManager.Positions.Record.StudentKey", studentKey);

            var domen = dto.Adapt<IEnumerable<Reception>>();

            return domen;
        }

        public async Task<IEnumerable<Reception>> GetAppointments(IEnumerable<Guid> studentKeys)
        {
            var dto = database.Receptions.Repository.FilterByArray("PositionManager.Positions.Record.StudentKey", studentKeys);

            var domen = dto.Adapt<IEnumerable<Reception>>();

            return domen;
        }

        public async Task<IEnumerable<Domain.Education.Student>> GetByKeys(IEnumerable<Guid> studentKeys)
        {
            var students = await lcservice.Student.GetStudentsByKeys(studentKeys);

            var result = students.Adapt<IEnumerable<Domain.Education.Student>>();

            return result;
        }
    }
}
