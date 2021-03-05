using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Model;
using Service.lC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Provider
{
    public class GroupProvider : GenericProvider<Group, GroupDto>
    {
        private readonly IManager manager;

        public GroupProvider(RepositoryDepository depository, IManager manager)
            : base(depository.Group, depository)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<Group>> FilterByProgram(IEnumerable<Guid> programKeys)
        {
            var today = DateTime.Now;

            var query = manager.Group
                        .Select(x=>x.Key)
                        .Filter(x => x.DeletionMark == false).AndAlso();
                        //.Filter(x => x.Finish > today).AndAlso();

            var nodeList = new LinkedList<Guid>(programKeys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.ProgramKey == value);
                if (node != nodeList.Last) query.Or();
            };

            var result = await query.GetByFilter();

            var keys = result?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var groups = await Repository.GetAsync(keys);

            return groups;
        }
    }
}
