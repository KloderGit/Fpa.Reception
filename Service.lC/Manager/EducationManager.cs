using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public partial class EducationManager
    {
        private readonly ProgramProvider programProvider;
        private readonly IProvider<Base, BaseDto> educationFormProvider;
        private readonly IProvider<Base, BaseDto> disciplinesProvider;
        private readonly IProvider<Base, BaseDto> controlTypeProvider;
        private readonly IProvider<Base, BaseDto> employeeProvider;
        private readonly IProvider<Base, BaseDto> groupProvider;
        private readonly IProvider<Base, BaseDto> subGroupProvider;

        private readonly IManager lcManager;

        public EducationManager(ProviderDepository depository, IManager lcManager)
        {
            this.programProvider = depository.Program;
            this.educationFormProvider = depository.EducationForm;
            this.disciplinesProvider = depository.Discipline;
            this.controlTypeProvider = depository.ControlType;
            this.employeeProvider = depository.Employee;
            this.groupProvider = depository.Group;
            this.subGroupProvider = depository.SubGroup;

            this.lcManager = lcManager;
        }

        public async Task<IEnumerable<Program>> GetTeacherPrograms(Guid teacherKey)
        {
            var foundedProgramKeys = await FindTeacherPrograms(teacherKey);

            var programs = await GetPrograms(foundedProgramKeys);

            return programs;
        }

        public async Task<IEnumerable<Program>> GetDisciplinePrograms(Guid disciplineKey)
        {
            var foundedProgramKeys = await FindDisciplinePrograms(disciplineKey);

            var programs = await GetPrograms(foundedProgramKeys);

            return programs;
        }

        public async Task<IEnumerable<Base>> GetProgramGroups(Guid programKey)
        {
            var foundedGroupKeys = await FindProgramGroups(programKey);

            var groups = await groupProvider.Repository.GetAsync(foundedGroupKeys);

            return groups;
        }

        public async Task<IEnumerable<Base>> GetGroupSubgroup(Guid groupKey)
        {
            var foundedSubGroupKeys = await FindGroupSubGroup(groupKey);

            var subGroups = await subGroupProvider.Repository.GetAsync(foundedSubGroupKeys);

            return subGroups;
        }
    }
}
