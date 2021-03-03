using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lcService = Service.lC;

namespace Application.Component
{
    public class EducationComponent
    {
        private readonly ProviderDepository providerDepository;

        public EducationComponent(ProviderDepository providerDepository)
        {
            this.providerDepository = providerDepository;
        }

        public async Task<IEnumerable<lcService.Model.Program>> GetProgramByTeacher(Guid teacherKey)
        {
            var foundedProgramKeys = await providerDepository.Program.FindTeacherPrograms(teacherKey);
            var receivedPrograms = await providerDepository.Program.Repository.GetAsync(foundedProgramKeys);
            var programs = receivedPrograms.ToList();

            var educationFormKeys = programs.Select(x => x.EducationForm.Key);
            var educationForms = await providerDepository.EducationForm.Repository.GetAsync(educationFormKeys);
            programs.ForEach(x => x.EducationForm = educationForms.FirstOrDefault(e=>e.Key == x.EducationForm.Key));

            var teacherKeys = programs.SelectMany(x => x.Teachers.Select(t => t.Key));
            var teachers = await providerDepository.Employee.Repository.GetAsync(teacherKeys);
            programs.ForEach( x => x.Teachers = teachers.Where(x => teacherKeys.Contains(x.Key)) );

            var disciplineKeys = programs.SelectMany(p => p.Educations.Select(d => d.Discipline.Key));
            var disciplines = await providerDepository.Discipline.Repository.GetAsync(disciplineKeys);
            var controlTypeKeys = programs.SelectMany(p => p.Educations.Select(d => d.ControlType.Key));
            var controlTypes = await providerDepository.ControlType.Repository.GetAsync(controlTypeKeys);            

            foreach (var program in programs)
            {
                var educations = program.Educations.Select(
                        x => new Education
                        {
                            Order = x.Order,
                            Discipline = disciplines.FirstOrDefault(d => d.Key == x.Discipline.Key),
                            ControlType = controlTypes.FirstOrDefault(c => c.Key == x.ControlType.Key)
                        }
                    );

                program.Educations = educations;
            }

            return programs;
        }


    }
}
