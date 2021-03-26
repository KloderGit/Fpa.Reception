using Service.lC.Dto;
using Service.lC.Extensions;
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

        public async Task<IEnumerable<Program>> GetAll()
        {
            var program = await programProvider.GetAll();

            return program;
        }

        public async Task<Program> GetProgram(Guid programKey)
        {
            var program = await programProvider.Repository.GetAsync(programKey);

            return program;
        }

        public async Task<IEnumerable<Program>> GetPrograms(IEnumerable<Guid> programKeys)
        {
            var programs = await programProvider.Repository.GetAsync(programKeys);

            return programs;
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

        public async Task IncludeDisciplines(IEnumerable<Program> programs)
        {
            if (programs.IsNullOrEmpty()) return;

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
        }

        public async Task IncludeEducationForm(IEnumerable<Program> programs)
        {
            if (programs.IsNullOrEmpty()) return;

            var educationFormKeys = ReduceArray(programs.Select(x => x.EducationForm.Key));
            var educationForms = await educationFormProvider.Repository.GetAsync(educationFormKeys);            

            programs.ToList()
                .ForEach(x => x.EducationForm = educationForms.FirstOrDefault(g => g.Key == x.EducationForm.Key));

            //array.ForEach(x => x.EducationForm = educationForms.FirstOrDefault(e => e.Key == x.EducationForm.Key));
        }

        public async Task IncludeTeachers(IEnumerable<Program> programs)
        {
            if (programs.IsNullOrEmpty()) return;

            var teacherKeys = ReduceArray( programs.SelectMany(x => x.Teachers.Select(t => t.Key)) );

            var teachers = await employeeProvider.Repository.GetAsync(teacherKeys);

            programs.ToList()
                .ForEach(x => x.Teachers = x.Teachers.Select(t=> teachers.FirstOrDefault(f=>f.Key == t.Key)));
        }

        public async Task IncludeGroups(IEnumerable<Program> programs)
        {
            if (programs.IsNullOrEmpty()) return;

            var programsKeys = ReduceArray(programs.Select(x => x.Key));

            var groups = await groupProvider.FilterByProgram(programsKeys);

            programs.ToList()
                .ForEach(x => x.Groups = groups.Where(g => g.Owner == x.Key));
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
