using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Extensions;
using Newtonsoft.Json;

namespace Application.HttpClient
{
    public class ProgramHttpClient : CommonHttpClient
    {
        public ProgramHttpClient(System.Net.Http.HttpClient client)
        :base(client: client)
        {}

        public async Task<IEnumerable<ProgramDto>> GetByDiscipline(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<ProgramDto>();

            var request = await Client.GetAsync("GetByDisciplines", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<ProgramDto>>();
            }

            return result.ToList();
        }
    }
}
