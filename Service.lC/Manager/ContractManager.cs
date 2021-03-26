using Service.lC.Extensions;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class ContractManager
    {
        private readonly ContractProvider contractProvider;
        private readonly ProgramProvider programProvider;
        private readonly GroupProvider groupProvider;
        private readonly SubGroupProvider subGroupProvider;

        public ContractManager(
            ContractProvider contractProvider,
            ProgramProvider programProvider,
            GroupProvider groupProvider,
            SubGroupProvider subGroupProvider
            )
        {
            this.contractProvider = contractProvider;
            this.programProvider = programProvider;
            this.groupProvider = groupProvider;
            this.subGroupProvider = subGroupProvider;
        }

        public async Task<Contract> Get(Guid contractKey)
        {
            var contract = await contractProvider.Repository.GetAsync(contractKey);

            return contract;
        }

        public async Task<IEnumerable<Contract>> GetByStudent(Guid studentKey)
        {
            var contracts = await contractProvider.FilterByStudent(new List<Guid> { studentKey });

            return contracts;
        }

        public async Task<IEnumerable<Contract>> FindForStudentByProgram(Guid studentKey, Guid programKey)
        {
            var contracts = await contractProvider.FilterByStudent(new List<Guid> { studentKey });

            var result = contracts.Where(x => x.EducationProgram.Key == programKey);

            return result;
        }

        public async Task IncludePrograms(IEnumerable<Contract> contracts)
        {
            var programKeys = ReduceArray(contracts.Select(t => t.EducationProgram.Key));
            if (programKeys.IsFilled())
            {
                var programs = await programProvider.Repository.GetAsync(programKeys);

                contracts.ToList()
                    .ForEach(x => x.EducationProgram = programs.FirstOrDefault(p=>p.Key == x.EducationProgram.Key));
            }
        }

        public async Task IncludeGroup(IEnumerable<Contract> contracts)
        {
            var groupKeys = ReduceArray(contracts.Select(t => t.Group.Key));

            if (groupKeys.IsFilled())
            {
                var groups = await groupProvider.Repository.GetAsync(groupKeys);

                contracts.ToList()
                    .ForEach(x => x.Group = groups.FirstOrDefault(p => p.Key == x.Group.Key));
            }
        }

        public async Task IncludeSubGroup(IEnumerable<Contract> contracts)
        {
            var subGroupKeys = ReduceArray(contracts.Select(t => t.SubGroup.Key));

            if (subGroupKeys.IsFilled())
            {
                var subGroups = await subGroupProvider.Repository.GetAsync(subGroupKeys);

                contracts.ToList()
                    .ForEach(x => x.SubGroup = subGroups.FirstOrDefault(p => p.Key == x.SubGroup.Key));
            }
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
