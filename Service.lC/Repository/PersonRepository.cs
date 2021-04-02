using Service.lC.Dto;
using Service.lC.Extensions;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Repository
{
    public class PersonRepository : GenericRepository<Person, PersonDto>
    {
        public PersonRepository(BaseHttpClient httpClient, string endpoint)
            :base(httpClient, endpoint)
        {}

        public async Task<IEnumerable<Person>> FindByContacts(IEnumerable<string> phones, IEnumerable<string> emails)
        {
            var result = Enumerable.Empty<Person>();

            var @params = new { Phones = phones, Emails = emails };

            var request = await http.Client.GetAsync(endpoint + "/" + "FindByContacts", @params);

            if (request.IsSuccessStatusCode)
            {
                var dto = await request.GetResultAsync<IEnumerable<PersonDto>>();

                var domain = dto.Select(x => x.ConvertTo<Person>(converter));

                result = domain ?? result;
            }

            return result;
        }
    }
}
