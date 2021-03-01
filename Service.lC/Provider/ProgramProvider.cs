using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Provider
{
    public class ProgramProvider: GenericProvider<Program>
    {
        private readonly IRepositoryAsync<Base> educationFormRepository;
        private readonly IRepositoryAsync<Base> emploeeRepository;
        private readonly IRepositoryAsync<Base> disciplineRepository;
        private readonly IRepositoryAsync<Base> controlTypeRepository;

        public ProgramProvider (RepositoryDepository depository)
            : base(depository.Program, depository)
        {
            this.educationFormRepository = depository.EducationForm;
            this.emploeeRepository = depository.Employee;
            this.disciplineRepository = depository.Discipline;
            this.controlTypeRepository = depository.ControlType;
        }
        
        public async Task<Program> IncludeEducationForm(Program program)
        {
            var key = program.EducationForm.Key;

            var educationForm = await educationFormRepository.GetAsync(key);

            program.EducationForm = educationForm;

            return program;
        }

        public async Task<IEnumerable<Program>> IncludeEducationForm(IEnumerable<Program> programs)
        {
            var keys = programs.Select(x=>x.EducationForm.Key);

            var educationForms = await educationFormRepository.GetAsync(keys);

            foreach (var program in programs)
            {
                var key = program.EducationForm.Key;
                program.EducationForm = educationForms.FirstOrDefault(x => x.Key == key);
            }

            return programs;
        }

        public async Task<Program> IncludeTeachers(Program program)
        {
            var keys = program.Teachers?.Select(x=>x.Key);

            var teachers = await emploeeRepository.GetAsync(keys);

            program.Teachers = teachers;

            return program;
        }

        public async Task<IEnumerable<Program>> IncludeTeachers(IEnumerable<Program> programs)
        {
            var keys = programs.SelectMany(x => x.Teachers.Select(k=>k.Key));

            var teachers = await emploeeRepository.GetAsync(keys);

            foreach (var program in programs)
            {
                var teacherKeys = program.Teachers.Select(x => x.Key);

                var programTeachers = teachers.Where(x => teacherKeys.Contains(x.Key));

                program.Teachers = programTeachers;
            }

            return programs;
        }

        public async Task<Program> IncludeEducations(Program program)
        {
            var disciplineKeys = program.Educations?.Select(x=>x.Discipline.Key);
            var controlTypeKeys = program.Educations?.Select(x=>x.ControlType.Key);

            var disciplines = await disciplineRepository.GetAsync(disciplineKeys);
            var controlTypes = await controlTypeRepository.GetAsync(controlTypeKeys);
            
            var scheme = program.Educations;

            var educations = scheme.Select(x => 
                    new Education
                    {
                        Discipline = disciplines.FirstOrDefault(d=>d.Key == x.Discipline.Key),
                        ControlType = controlTypes.FirstOrDefault(ct=>ct.Key == x.ControlType.Key)
                    }
                );

            program.Educations = educations;

            return program;
        }

        public async Task<IEnumerable<Program>> IncludeEducations(IEnumerable<Program> programs)
        {
            var disciplineKeys = programs.SelectMany(x => x.Educations.Select(d => d.Discipline.Key));
            var controlTypeKeys = programs.SelectMany(x => x.Educations.Select(d => d.ControlType.Key));

            var disciplines = await disciplineRepository.GetAsync(disciplineKeys);
            var controlTypes = await controlTypeRepository.GetAsync(controlTypeKeys);

            foreach (var program in programs)
            {
                var educations = program.Educations.Select(
                        x=> new Education { 
                            Discipline = disciplines.FirstOrDefault(d=>d.Key == x.Discipline.Key), 
                            ControlType = controlTypes.FirstOrDefault(c=>c.Key == x.ControlType.Key) }
                    );

                program.Educations = educations;
            }

            return programs;
        }
    }
}