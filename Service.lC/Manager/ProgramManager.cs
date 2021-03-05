using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class ProgramManager
    {
        private readonly ProgramProvider programProvider;
        private readonly IProvider<Base, BaseDto> disciplineProvider;
        private readonly IProvider<Base, BaseDto> controltypeProvider;
        private readonly IProvider<Base, BaseDto> educationFormProvider;
        private readonly IProvider<Base, BaseDto> employeeProvider;
        private readonly GroupProvider groupProvider;

        public ProgramManager(
            ProgramProvider programProvider,
            IProvider<Base, BaseDto> disciplineProvider,
            IProvider<Base, BaseDto> controltypeProvider,
            IProvider<Base, BaseDto> educationFormProvider,
            IProvider<Base, BaseDto> employeeProvider,
            GroupProvider groupProvider
            )
        {
            this.programProvider = programProvider;
            this.disciplineProvider = disciplineProvider;
            this.controltypeProvider = controltypeProvider;
            this.educationFormProvider = educationFormProvider;
            this.employeeProvider = employeeProvider;
            this.groupProvider = groupProvider;
        }

        public async Task<IEnumerable<Program>> FilterByTeacher(Guid teacherKey)
        {
            var programs = await programProvider.FilterByTeacher(teacherKey);

            return programs;
        }

        public async Task<IEnumerable<Program>> FilterByDiscipline(Guid disciplineKey)
        {
            var programs = await programProvider.FilterByDiscipline(disciplineKey);

            return programs;
        }

        public async Task<IEnumerable<Program>> IncludeDisciplines(IEnumerable<Program> programs)
        {
            var array = programs.ToList();

            var disciplineKeys = ReduceArray(array.SelectMany(p => p.Educations.Select(d => d.Discipline.Key)));
            var disciplines = await disciplineProvider.Repository.GetAsync(disciplineKeys);

            var controlTypeKeys = ReduceArray(array.SelectMany(p => p.Educations.Select(d => d.ControlType.Key)));
            var controlTypes = await controltypeProvider.Repository.GetAsync(controlTypeKeys);

            array.ForEach(p =>
            {
                var eduScheme = p.Educations.ToList();
                eduScheme.ForEach(e =>
                {
                    e.ControlType = controlTypes.FirstOrDefault(c => c.Key == e.ControlType.Key);
                    e.Discipline = disciplines.FirstOrDefault(d => d.Key == e.Discipline.Key);
                });
                p.Educations = eduScheme;
            });

            return array;
        }

        public async Task<IEnumerable<Program>> IncludeEducationForm(IEnumerable<Program> programs)
        {
            var array = programs.ToList();

            var educationFormKeys = ReduceArray(array.Select(x => x.EducationForm.Key));
            var educationForms = await educationFormProvider.Repository.GetAsync(educationFormKeys);
            array.ForEach(x => x.EducationForm = educationForms.FirstOrDefault(e => e.Key == x.EducationForm.Key));

            return array;
        }

        public async Task<IEnumerable<Program>> IncludeTeachers(IEnumerable<Program> programs)
        {
            var teacherKeys = ReduceArray( programs.SelectMany(x => x.Teachers.Select(t => t.Key)) );

            var teachers = await employeeProvider.Repository.GetAsync(teacherKeys);

            programs.ToList()
                .ForEach(x => x.Teachers = teachers.Where(x => teacherKeys.Contains(x.Key)));

            return programs;
        }

        public async Task<IEnumerable<Program>> IncludeGroups(IEnumerable<Program> programs)
        {
            var programsKeys = ReduceArray(programs.SelectMany(x => x.Teachers.Select(t => t.Key)));

            var groups = await groupProvider.FilterByProgram(programsKeys);

            programs.ToList()
                .ForEach(x => x.Groups = groups.Where(g => g.Owner == x.Key));

            return programs;
        }

        private List<Guid> ReduceArray(IEnumerable<Guid> keys)
        {
            return keys
                .Distinct()
                .Where(x => x != default)
                .ToList();
        }
    }
}
