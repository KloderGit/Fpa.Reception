using Domain;
using Domain.Interface;
using Mapster;
using Service.lC;
using Service.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Application.Component
{
    public class TeacherComponent : ITeacherComponent
    {
        private readonly MongoContext database;
        private readonly Context lcService;

        public TeacherComponent(MongoContext database, Context lcService)
        {
            this.database = database;
            this.lcService = lcService;
        }

        public async Task<IEnumerable<Domain.Education.Program>> GetEducation(Guid employeeKey)
        {
            var foundedProgramsQuery = await lcService.Program.FilterByTeacher(employeeKey);
            await lcService.Program.IncludeDisciplines(foundedProgramsQuery);
            await lcService.Program.IncludeEducationForm(foundedProgramsQuery);

            var domain = foundedProgramsQuery.Adapt<IEnumerable<Domain.Education.Program>>();

            return domain;
        }

        public async Task<IEnumerable<Reception>> GetReceptions(Guid employeeKey, Guid disciplineKey, DateTime fromDate, DateTime toDate)
        {
            var foundedReceptions = await database.Receptions.GetByTeacherAndDiscipline(employeeKey, disciplineKey, fromDate, toDate);

            var domain = foundedReceptions.Adapt<IEnumerable<Domain.Reception>>();

            return domain;
        }
    }
}
