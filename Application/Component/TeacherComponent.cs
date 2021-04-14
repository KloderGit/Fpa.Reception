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
    public class TeacherComponent : ITeacherComponent
    {
        private readonly Context lcService;

        public TeacherComponent(Context lcService)
        {
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

        public Task<IEnumerable<Reception>> GetReceptions(Guid employeeKey, Guid disciplineKey, DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }
    }
}
