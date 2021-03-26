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
    public class StudentComponent : IStudentComponent
    {
        private readonly Context lcservice;
        private readonly MongoContext database;

        public StudentComponent(MongoContext mongo, Context lcservice)
        {
            this.lcservice = lcservice;
            this.database = mongo;
        }

        public async Task<dynamic> GetAttestation(Guid studentKey, Guid programKey)
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

            var dto = database.Receptions.FilterByArray("Events.Discipline.Key", disciplines).ToList();



            var domen = dto.Adapt<List<Reception>>();

            // Получить все рецепции по дисциплине и под ограничения которых попадает студент
            // и на которые он еще не записан

            // get reception.where(Events.Contain(disciplineKey))
            //              .where(Restriction.Program == program || null)
            //              .where(Restriction.Group == group || null)
            //              .where(Restriction.SubGroup == subGroup || null)
            //              .where(Position.Any(studentKey) == false)



            return domen;
        }
    }
}
