using Service.lC.Manager;
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
            var manager = new ProgramManager(providerDepository);

            var progs = await manager.FilterByTeacher(teacherKey);
                progs = await manager.IncludeDisciplines(progs);
                progs = await manager.IncludeEducationForm(progs);
                progs = await manager.IncludeTeachers(progs);
                progs = await manager.IncludeGroups(progs);

            return progs;
        }

        public async Task<IEnumerable<lcService.Model.Program>> GetProgramByDiscipline(Guid disciplineKey)
        {
            var manager = new ProgramManager(providerDepository);

            var progs = await manager.FilterByTeacher(disciplineKey);
                progs = await manager.IncludeEducationForm(progs);
                progs = await manager.IncludeTeachers(progs);

            return progs;
        }
    }
}
