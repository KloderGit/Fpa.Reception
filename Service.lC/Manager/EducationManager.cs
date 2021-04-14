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
        private readonly IProvider<Base, BaseDto> controltypeProvider;

        public EducationManager(
            IProvider<Base, BaseDto> disciplineProvider,
            IProvider<Base, BaseDto> controltypeProvider
            )
        {
            this.disciplineProvider = disciplineProvider;
            this.controltypeProvider = controltypeProvider;
        }

        public async Task<IEnumerable<Base>> GetDisciplinesByKeys( IEnumerable<Guid> disciplineKeys)
        {
            var disciplines = await disciplineProvider.Repository.GetAsync(disciplineKeys);

            return disciplines;
        }

        public async Task<IEnumerable<Base>> GetControlTypesByKeys(IEnumerable<Guid> controlTypeKeys)
        {
            var controlTypes = await controltypeProvider.Repository.GetAsync(controlTypeKeys);

            return controlTypes;
        }

    }
}
