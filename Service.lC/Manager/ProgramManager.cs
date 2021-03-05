using lc.fitnesspro.library.Interface;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class ProgramManager
    {
        private readonly ProviderDepository depository;

        public ProgramManager(ProviderDepository depository)
        {
            this.depository = depository;
        }

        public async Task<IEnumerable<Program>> FilterByTeacher(Guid teacherKey)
        {
            var programs = await depository.Program.FilterByTeacher(teacherKey);

            return programs;
        }

        public async Task<IEnumerable<Program>> FilterByDiscipline(Guid disciplineKey)
        {
            var programs = await depository.Program.FilterByDiscipline(disciplineKey);

            return programs;
        }

        public async Task<IEnumerable<Program>> IncludeDisciplines(IEnumerable<Program> programs)
        {
            var array = programs.ToList();

            var disciplineKeys = ReduceArray(array.SelectMany(p => p.Educations.Select(d => d.Discipline.Key)));
            var disciplines = await depository.Discipline.Repository.GetAsync(disciplineKeys);

            var controlTypeKeys = ReduceArray(array.SelectMany(p => p.Educations.Select(d => d.ControlType.Key)));
            var controlTypes = await depository.ControlType.Repository.GetAsync(controlTypeKeys);

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
            var educationForms = await depository.EducationForm.Repository.GetAsync(educationFormKeys);
            array.ForEach(x => x.EducationForm = educationForms.FirstOrDefault(e => e.Key == x.EducationForm.Key));

            return array;
        }

        public async Task<IEnumerable<Program>> IncludeTeachers(IEnumerable<Program> programs)
        {
            var teacherKeys = ReduceArray( programs.SelectMany(x => x.Teachers.Select(t => t.Key)) );

            var teachers = await depository.Employee.Repository.GetAsync(teacherKeys);

            programs.ToList()
                .ForEach(x => x.Teachers = teachers.Where(x => teacherKeys.Contains(x.Key)));

            return programs;
        }

        public async Task<IEnumerable<Program>> IncludeGroups(IEnumerable<Program> programs)
        {
            var programsKeys = ReduceArray(programs.SelectMany(x => x.Teachers.Select(t => t.Key)));

            var groups = await depository.Group.FilterByProgram(programsKeys);

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
