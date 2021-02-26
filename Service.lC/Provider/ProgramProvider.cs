using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lc.fitnesspro.library;
using Service.lC.Interface;
using Service.lC.Model;

namespace Service.lC.Provider
{
    public class ProgramProvider : IRepositoryAsync<Program>
    {
        private readonly IRepositoryAsync<Program> programRepository;
        private readonly IRepositoryAsync<EducationForm> educationFormRepository;
        private readonly IRepositoryAsync<Base> emploeeRepository;
        private readonly IRepositoryAsync<Discipline> disciplineRepository;
        private readonly IRepositoryAsync<ControlType> controlTypeRepository;

        public ProgramProvider(
            IRepositoryAsync<Program> programRepository,
            IRepositoryAsync<EducationForm> educationFormRepository,
            IRepositoryAsync<Base> emploeeRepository,
            IRepositoryAsync<Discipline> disciplineRepository,
            IRepositoryAsync<ControlType> controlTypeRepository
            )
        {
            this.programRepository = programRepository;
            this.educationFormRepository = educationFormRepository;
            this.emploeeRepository = emploeeRepository;
            this.disciplineRepository = disciplineRepository;
            this.controlTypeRepository = controlTypeRepository;
        }
        
        public async Task<IEnumerable<Program>> GetAsync()
        {
            return await programRepository.GetAsync();
        }

        public async Task<Program> GetAsync(Guid key)
        {
            return await programRepository.GetAsync(key);
        }

        public async Task<IEnumerable<Program>> GetAsync(IEnumerable<Guid> keys)
        {
            return await programRepository.GetAsync(keys);
        }

        public async Task<Program> IncludeEducationForm(Program program)
        {
            var key = program.EducationForm.Key;

            var educationForm = await educationFormRepository.GetAsync(key);

            program.EducationForm = educationForm;

            return program;
        }
        
        public async Task<Program> IncludeTeachers(Program program)
        {
            var keys = program.Teachers?.Select(x=>x.Key);

            var teachers = await emploeeRepository.GetAsync(keys);

            program.Teachers = teachers;

            return program;
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
    }
}