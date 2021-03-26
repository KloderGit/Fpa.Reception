using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Extensions;
using Service.lC.Interface;
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

        public GroupProvider(
            IRepositoryAsync<Group, GroupDto> repository,
            IManager manager)
            : base(repository)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<Group>> FilterByProgram(IEnumerable<Guid> programKeys)
        {
            if (programKeys.IsNullOrEmpty()) return new List<Group>();


            var lcGroups = new List<lc.fitnesspro.library.Model.Group>();


            var chekedFullQuery = BuildQuery(programKeys);

            var dbg = chekedFullQuery.DebugViewQuery();


            if (chekedFullQuery.IsQueryLengthMoreThen(3000))
            {
                manager.Group.ClearQuery();

                var piece = 30;
                var cnt = Math.Round((decimal)programKeys.Count() / piece);
                var indx = 0;

                for (var i = 0; i < cnt; i++)
                {
                    var array = programKeys.Skip(indx).Take(piece).ToArray();
                    indx += piece;

                    var query = BuildQuery(array);

                    dbg = chekedFullQuery.DebugViewQuery();

                    var response = await query.GetByFilter();

                    lcGroups.AddRange(response);
                }
            }

            var keys = lcGroups?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var groups = await Repository.GetAsync(keys);

            return groups;
        }

        private IRepository<lc.fitnesspro.library.Model.Group> BuildQuery(IEnumerable<Guid> keys)
        {
            var today = DateTime.Now;

            var query = manager.Group
                        .Select(x => x.Key)
                        .Filter(x => x.DeletionMark == false).And()
                        .Filter(x => x.Finish > today).AndAlso();

            var nodeList = new LinkedList<Guid>(keys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.ProgramKey == value);
                if (node != nodeList.Last) query.Or();
            };

            return query;
        }
    }
}
