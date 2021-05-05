using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class EducationManager
    {
        private readonly IProvider<Base, BaseDto> disciplineProvider;
        private readonly IProvider<Base, BaseDto> employeeProvider;

        public EducationManager(
            IProvider<Base, BaseDto> disciplineProvider,
            IProvider<Base, BaseDto> employeeProvider
            )
        {
            this.disciplineProvider = disciplineProvider;
            this.employeeProvider = employeeProvider;
        }

        public async Task<IEnumerable<Base>> GetDisciplinesByKeys( IEnumerable<Guid> disciplineKeys)
        {
            var disciplines = await disciplineProvider.Repository.GetAsync(disciplineKeys);

            return disciplines;
        }

        public async Task<IEnumerable<Base>> GetTeachersByKeys(IEnumerable<Guid> teacherKeys)
        {
            var teachers = await employeeProvider.Repository.GetAsync(teacherKeys);

            return teachers;
        }
    }
}
