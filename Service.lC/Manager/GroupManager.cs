using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class GroupManager
    {
        private readonly SubGroupProvider subGroupProvider;

        public GroupManager(
            SubGroupProvider subGroupProvider
            )
        {
            this.subGroupProvider = subGroupProvider;
        }

        public async Task<IEnumerable<Group>> IncludeSubGroups(IEnumerable<Group> groups)
        {
            var groupKeys = ReduceArray(groups.Select(t => t.Key));

            var subGroups = await subGroupProvider.FilterByGroup(groupKeys);

            groups.ToList()
                .ForEach(x => x.SubGroups = subGroups.Where(g => g.Owner == x.Key));

            return groups;
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
