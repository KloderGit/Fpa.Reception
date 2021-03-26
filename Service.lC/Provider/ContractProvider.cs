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
    public class ContractProvider : GenericProvider<Contract, ContractDto>
    {
        private readonly IManager manager;

        public ContractProvider(
            IRepositoryAsync<Contract, ContractDto> repository,
            IManager manager)
            : base(repository)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<Contract>> FilterByStudent(IEnumerable<Guid> studentKeys)
        {
            if (studentKeys.IsNullOrEmpty()) return new List<Contract>();

            var query = manager.Contract
                        .Select(x => x.Key)
                        .Filter(x => x.DeletionMark == false).AndAlso();

            var nodeList = new LinkedList<Guid>(studentKeys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.Registry.Any(t => t.StudentKey == value));
                if (node != nodeList.Last) query.Or();
            };

            var result = await query.GetByFilter();

            var keys = result?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var contracts = await Repository.GetAsync(keys);

            return contracts;
        }
    }
}
