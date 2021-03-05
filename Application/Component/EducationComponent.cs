using Service.lC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using lcService = Service.lC;

namespace Application.Component
{
    public class EducationComponent
    {
        private readonly Context lcService;

        public EducationComponent(Context lcService)
        {
            this.lcService = lcService;
        }

        public async Task<IEnumerable<lcService.Model.Program>> FindProgramByTeacher(Guid teacherKey)
        {
            var manager = lcService.Program;

            var programs = await manager.FilterByTeacher(teacherKey);
                programs = await manager.IncludeDisciplines(programs);
                programs = await manager.IncludeEducationForm(programs);
                programs = await manager.IncludeTeachers(programs);
                programs = await manager.IncludeGroups(programs);

            return programs;
        }

        public async Task<IEnumerable<lcService.Model.Program>> FindProgramByDiscipline(Guid disciplineKey)
        {
            var manager = lcService.Program;

            var programs = await manager.FilterByTeacher(disciplineKey);
                programs = await manager.IncludeTeachers(programs);
                programs = await manager.IncludeGroups(programs);

            return programs;
        }
    }
}
