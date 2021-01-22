using Application.HttpClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Person
{
    public class PersonMethods
    {
        PersonHttpClient http;
        public PersonMethods(PersonHttpClient client)
        {
            http = client;
        }

        public async Task<IEnumerable<Guid>> GetByContacts(IEnumerable<string> phones, IEnumerable<string> emails)
        {
            return await http.FindByContacts(phones, emails);
        }
    }
}
