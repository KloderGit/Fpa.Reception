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

        public EducationManager(
            IProvider<Base, BaseDto> disciplineProvider
            )
        {
            this.disciplineProvider = disciplineProvider;
        }

        public async Task<IEnumerable<Base>> GetDisciplinesByKeys( IEnumerable<Guid> disciplineKeys)
        {
            var disciplines = await disciplineProvider.Repository.GetAsync(disciplineKeys);

            return disciplines;
        }

    }
}
