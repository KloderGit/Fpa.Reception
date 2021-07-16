using lc.fitnesspro.library.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class AttestationTableManager
    {
        private readonly AttestationTableProvider attestationTableProvider;

        public AttestationTableManager(AttestationTableProvider attestationTableProvider)
        {
            if (attestationTableProvider is null)
            {
                throw new ArgumentNullException(nameof(attestationTableProvider));
            }

            this.attestationTableProvider = attestationTableProvider;
        }

        public async Task<IEnumerable<Model.AttestationTable>> FindByProgramAndDiscipline(Guid programKey, Guid disciplineKey)
        { 
            var result = await attestationTableProvider.FindByProgramAndDiscipline(programKey, disciplineKey);

            return result;
        }

        public async Task<Model.AttestationTable> Create(Model.AttestationTable attestationTable)
        { 
            var result = await attestationTableProvider.Create(attestationTable);

            return result;
        }
    }
}
