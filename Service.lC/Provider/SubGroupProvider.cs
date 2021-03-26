using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Extensions;
using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Provider
{
    public class SubGroupProvider : GenericProvider<SubGroup, SubGroupDto>
    {
        private readonly IManager manager;

        public SubGroupProvider(
            IRepositoryAsync<SubGroup, SubGroupDto> repository,
            IManager manager)
            : base(repository)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<SubGroup>> FilterByGroup(IEnumerable<Guid> groupKeys)
        {
            if (groupKeys.IsNullOrEmpty()) return new List<SubGroup>();

            var query = manager.SubGroup
                        .Filter(x => x.DeletionMark == false).AndAlso();

            var nodeList = new LinkedList<Guid>(groupKeys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.GroupKey == value);
                if (node != nodeList.Last) query.Or();
            };

            var result = await query.GetByFilter();

            var keys = result?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var subGroups = await Repository.GetAsync(keys);

            return subGroups;
        }
    }
}
