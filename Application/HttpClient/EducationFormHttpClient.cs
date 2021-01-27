using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Employee;
using Application.Extensions;
using Newtonsoft.Json;

namespace Application.HttpClient
{
    public class EducationFormHttpClient : CommonHttpClient
    {
        public EducationFormHttpClient(System.Net.Http.HttpClient client)
        :base(client: client)
        {}

        
        public async Task<IEnumerable<BaseInfoDto>> GetByKeys(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<BaseInfoDto>();

            var request = await Client.GetAsync("Find", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<BaseInfoDto>>();
            }

            return result;
        }

    }
}
