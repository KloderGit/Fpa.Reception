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

        public async Task<IEnumerable<Domain.Education.Program>> GetEducation(Guid key)
        {
            var programManager = lcService.Program;

            var programs = await programManager.FilterByTeacher(key);
            await programManager.IncludeDisciplines(programs);
            await programManager.IncludeEducationForm(programs);
            await programManager.IncludeTeachers(programs);
            await programManager.IncludeGroups(programs);

            var groupManager = lcService.Group;

            var groups = programs.SelectMany(x => x.Groups);
            await groupManager.IncludeSubGroups(groups);

            var domain = programs.Adapt<IEnumerable<Domain.Education.Program>>();

            return domain;
        }
    }
}
