using Application.HttpClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Program
{
    public class ProgramMethods
    {
        private readonly ProgramHttpClient http;


        public ProgramMethods(ProgramHttpClient client)
        {
            http = client;
        }

        public async Task<IEnumerable<ProgramDto>> GetByDiscipline(IEnumerable<Guid> keys)
        {
            return await http.FindByDiscipline(keys);
        }

        public async Task<IEnumerable<ProgramDto>> Find(IEnumerable<Guid> keys)
        {
            return await http.Find(keys);
        }
    }
}
