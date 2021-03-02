using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Provider
{
    public class ProgramProvider: GenericProvider<Program, ProgramDto>
    {
        private readonly IRepositoryAsync<Base,BaseDto> educationFormRepository;
        private readonly IRepositoryAsync<Base, BaseDto> emploeeRepository;
        private readonly IRepositoryAsync<Base, BaseDto> disciplineRepository;
        private readonly IRepositoryAsync<Base, BaseDto> controlTypeRepository;

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
            var array = programs.ToArray();

            var keys = array.Select(x=>x.EducationForm.Key).Distinct();

            var educationForms = await educationFormRepository.GetAsync(keys);

            for (int i=0; i < array.Count(); i++)
            {
                var key = array[i].EducationForm.Key;
                array[i].EducationForm = educationForms.FirstOrDefault(x => x.Key == key);
            }

            return array;
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
            var array = programs.ToArray();

            var keys = programs.SelectMany(x => x.Teachers.Select(k=>k.Key)).Distinct();

            var teachers = await emploeeRepository.GetAsync(keys);

            for (int i = 0; i < array.Count(); i++)
            {
                var teacherKeys = array[i].Teachers.Select(x => x.Key);

                var programTeachers = teachers.Where(x => teacherKeys.Contains(x.Key));

                array[i].Teachers = programTeachers;
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
            var disciplineKeys = programs.SelectMany(x => x.Educations.Select(d => d.Discipline.Key)).Distinct();
            var controlTypeKeys = programs.SelectMany(x => x.Educations.Select(d => d.ControlType.Key)).Distinct();

            var disciplines = await disciplineRepository.GetAsync(disciplineKeys);
            var controlTypes = await controlTypeRepository.GetAsync(controlTypeKeys);

            foreach (var program in programs)
            {
                var educations = program.Educations.Select(
                        x=> new Education { 
                            Order = x.Order,
                            Discipline = disciplines.FirstOrDefault(d=>d.Key == x.Discipline.Key), 
                            ControlType = controlTypes.FirstOrDefault(c=>c.Key == x.ControlType.Key) }
                    );

                program.Educations = educations;
            }

            return programs;
        }
    }
}