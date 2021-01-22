using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Extensions;
using Newtonsoft.Json;

namespace Application.HttpClient
{
    public class PersonHttpClient : CommonHttpClient
    {
        public PersonHttpClient(System.Net.Http.HttpClient client)
        :base(client: client)
        {}

        public async Task<IEnumerable<Guid>> FindByContacts(IEnumerable<string> phones, IEnumerable<string> emails)
        {
            var result = Enumerable.Empty<Guid>();

            var dto = new { Phones = phones, Emails = emails };

            var request = await Client.GetAsync("FindByContacts", dto);

            if (request.IsSuccessStatusCode)
            {
                var queryResult = await request.GetResultAsync<IEnumerable<PersonDto>>();

                if (queryResult.IsNullOrEmpty() == false) result = queryResult.Select(x => x.Key);
            }

            return result.ToList();
        }

        public class PersonDto
        {
            [JsonProperty("key")]
            public Guid Key { get; set; }
        }
    }
}
