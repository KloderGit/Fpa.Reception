using Application.Extensions;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.HttpClient
{
    public class DisciplineHttpClient : CommonHttpClient
    {
        public DisciplineHttpClient(System.Net.Http.HttpClient client)
        :base(client: client)
        {}

        public async Task<IEnumerable<BaseInfo>> Find(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<BaseInfo>();

            var request = await Client.GetAsync("Find", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<BaseInfo>>();
            }

            return result;
        }
    }
}
