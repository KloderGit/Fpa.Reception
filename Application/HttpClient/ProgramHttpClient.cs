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

        public async Task<IEnumerable<ProgramDto>> FindByDiscipline(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<ProgramDto>();

            var request = await Client.GetAsync("FindByDiscipline", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<ProgramDto>>();
            }

            return result.ToList();
        }

        public async Task<IEnumerable<ProgramDto>> FindByEmployee(Guid keys)
        {
            var result = Enumerable.Empty<ProgramDto>();

            var request = await Client.GetAsync("FindByDiscipline", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<ProgramDto>>();
            }

            return result.ToList();
        }

        public async Task<IEnumerable<ProgramDto>> GetAllPrograms()
        {
            var result = Enumerable.Empty<ProgramDto>();

            var request = await Client.GetAsync("/");

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<ProgramDto>>();
            }

            return result.ToList();
        }

        public async Task<IEnumerable<ProgramDto>> Find(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<ProgramDto>();

            var request = await Client.GetAsync("Find", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<ProgramDto>>();
            }

            return result.ToList();
        }
    }
}
