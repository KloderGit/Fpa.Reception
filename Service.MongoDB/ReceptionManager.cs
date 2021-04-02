using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.MongoDB
{
    public class ReceptionManager
    {
        public ReceptionProvider Provider;

        public ReceptionManager(ReceptionProvider provider)
        {
            this.Provider = provider;
        }

        public async Task<IEnumerable<Reception>> GetByDiscipline(Guid disciplineKey)
        {
            var result = await Task.Run( () => Provider.Repository.FilterByPath("Event.Discipline", disciplineKey) );

            return result;
        }
    }
}
