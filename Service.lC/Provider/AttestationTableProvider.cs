using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Model;
using Service.lC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC.Provider
{
    public class AttestationTableProvider : GenericProvider<AttestationTable, AttestationTableDto>
    {
        private readonly AttestationTableRepository repository;
        private readonly IManager manager;

        public AttestationTableProvider(
            AttestationTableRepository repository,
            IManager manager)
            : base(repository)
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (manager is null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            this.repository = repository;
            this.manager = manager;
        }

        public async Task<IEnumerable<AttestationTable>> FindByProgramAndDiscipline(Guid programKey, Guid disciplineKey)
        {
            var query = await manager.AttestationTable
                .Filter(x => x.ProgramKey == programKey).And()
                .Filter(x => x.DisciplineKey == disciplineKey)
                .Select(x => x.Key)
                .GetByFilter();

            var keys = query.Select(x => x.Key).ToList();

            var result = await Repository.GetAsync(keys);

            return result;
        }

        public async Task<AttestationTable> Create(AttestationTable attestationTable)
        {
            var result = await repository.Create(attestationTable);

            return result;
        }
    }
}
