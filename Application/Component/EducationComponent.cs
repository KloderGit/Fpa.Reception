using Domain.Interface;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lcService = Service.lC;

namespace Application.Component
{
    public class EducationComponent : IEducationComponent
    {
        private readonly Context lcService;

        public EducationComponent(Context lcService)
        {
            this.lcService = lcService;
        }

        public async Task<IEnumerable<Domain.Education.Program>> FindProgramByTeacher(Guid teacherKey)
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

            var domain = programs.Adapt<IEnumerable<Domain.Education.Program>>();

            return domain;
        }

        public async Task<IEnumerable<Domain.Education.Program>> FindProgramByDiscipline(Guid disciplineKey)
        {
            var programManager = lcService.Program;

            var programs = await programManager.FilterByDiscipline(disciplineKey);
                await programManager.IncludeTeachers(programs);
                await programManager.IncludeGroups(programs);

            var groupManager = lcService.Group;
            var groups = programs.SelectMany(x => x.Groups);
                await groupManager.IncludeSubGroups(groups);

            var domain = programs.Adapt<IEnumerable<Domain.Education.Program>>();

            return domain;
        }

        public async Task<IEnumerable<dynamic>> GetStudentEducationInfoByPersonKeys(IEnumerable<Guid> personKeys)
        {
            var personsPipe = lcService.Person;
            var studentsPipe = lcService.Student;
            var contractPipe = lcService.Contract;

            var persons = await personsPipe.FindByKeys(personKeys);
                await personsPipe.IncludeStudents(persons);
                await studentsPipe.IncludeContracts(persons.SelectMany(x=>x.Students));

            var contracts = persons.SelectMany(p => p.Students.SelectMany(s => s.Contract));
                await contractPipe.IncludePrograms(contracts);
                await contractPipe.IncludeGroup(contracts);
                await contractPipe.IncludeSubGroup(contracts);

            return persons;
        }
    }
}
