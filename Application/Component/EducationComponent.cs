﻿using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var programManager = lcService.Program;

            var programs = await programManager.FilterByTeacher(teacherKey);
                await programManager.IncludeDisciplines(programs);
                await programManager.IncludeEducationForm(programs);
                await programManager.IncludeTeachers(programs);
                await programManager.IncludeGroups(programs);

            var groupManager = lcService.Group;

            var groups = programs.SelectMany(x => x.Groups);
                await groupManager.IncludeSubGroups(groups);

            return programs;
        }

        public async Task<IEnumerable<lcService.Model.Program>> FindProgramByDiscipline(Guid disciplineKey)
        {
            var programManager = lcService.Program;

            var programs = await programManager.FilterByDiscipline(disciplineKey);
                await programManager.IncludeTeachers(programs);
                await programManager.IncludeGroups(programs);

            var groupManager = lcService.Group;
            var groups = programs.SelectMany(x => x.Groups);
                await groupManager.IncludeSubGroups(groups);

            return programs;
        }
    }
}
