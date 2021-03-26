using Service.lC.Extensions;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task IncludeSubGroups(IEnumerable<Group> groups)
        {
            if (groups.IsNullOrEmpty()) return;

            var groupKeys = ReduceArray(groups.Select(t => t.Key));

            var subGroups = await subGroupProvider.FilterByGroup(groupKeys);

            groups.ToList()
                .ForEach(x => x.SubGroups = subGroups.Where(g => g.Owner == x.Key));
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
