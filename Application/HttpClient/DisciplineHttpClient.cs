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
        : base(client: client)
        { }

        public async Task<IEnumerable<BaseInfo>> Find(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<BaseInfo>();

            var array = keys.Select((s, i) => keys.Skip(i * 50).Take(50)).Where(a => a.Any());

            foreach (var part in array)
            {
                var request = await Client.GetAsync("Find", part);

                if (request.IsSuccessStatusCode)
                {
                    var query = await request.GetResultAsync<IEnumerable<BaseInfo>>();

                    result = result.Concat(query);
                }
            }

            return result;
        }
    }
}
