using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Extensions;
using Service.lC.Model;
using Service.lC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Provider
{
    public class PersonProvider : GenericProvider<Person, PersonDto>
    {
        private readonly IManager manager;

        public PersonProvider(
            PersonRepository repository,
            IManager manager)
            : base(repository)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<Person>> FindByContacts(IEnumerable<string> phones, IEnumerable<string> emails)
        {
            var repository = Repository as Service.lC.Repository.PersonRepository;

            var persons = await repository.FindByContacts(phones, emails);

            var keys = persons.Select(x => x.Key);

            //var persons2 = await manager.Person.GetByContacts(new PersonContactDto { phones = phones, emails = emails });

            return persons.ToList();
        }

        public async Task<IEnumerable<Person>> FindByQuery(string marker)
        {
            var queryString = @$"?$format=json&$filter=substringof('{marker}', КонтактнаяИнформация/НомерТелефона) "
                                + @$"or substringof('{marker}', КонтактнаяИнформация/АдресЭП) "
                                + @$"or substringof('{marker}', Description)"
                                + "&$select=Ref_Key, Description, ЛогинСкайп, "
                                + "КонтактнаяИнформация/НомерТелефона, КонтактнаяИнформация/АдресЭП";

            var repository = Repository as Service.lC.Repository.PersonRepository;

            var persons = await repository.GetByQueryAsync(queryString);

            return persons;
        }
    }
}
