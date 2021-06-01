using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class PersonManager
    {
        private readonly PersonProvider personProvider;
        private readonly StudentProvider studentProvider;

        public PersonManager(
            PersonProvider personProvider,
            StudentProvider studentProvider
            )
        {
            this.personProvider = personProvider;
            this.studentProvider = studentProvider;
        }

        public async Task<Person> Get(Guid key)
        {
            var person = await personProvider.Repository.GetAsync(key);

            return person;
        }

        public async Task<IEnumerable<Person>> Get(IEnumerable<Guid> keys)
        {
            var persons = await personProvider.Repository.GetAsync(keys);

            return persons;
        }

        public async Task<IEnumerable<Person>> FindByContacts(IEnumerable<string> phones, IEnumerable<string> emails)
        {
            var persons = await personProvider.FindByContacts(phones, emails);

            return persons;
        }

        public async Task<IEnumerable<Person>> FindByQuery(string queryString)
        {
            var persons = await personProvider.FindByQuery(queryString);

            return persons;
        }

        public async Task<IEnumerable<Person>> FindByStudents(IEnumerable<Guid> studentKeys)
        {
            var students = await studentProvider.Repository.GetAsync(studentKeys);

            var personsKeys = students.Select(x => x.Owner);

            var persons = await personProvider.Repository.GetAsync(personsKeys);

            return persons;
        }

        public async Task IncludeStudents(IEnumerable<Person> persons)
        {
            var personKeys = persons.Select(x => x.Key).ToList().Distinct();
            var students = await studentProvider.FilterByPerson(personKeys);

            persons.ToList()
                .ForEach(x => x.Students = students.Where(s => s.Owner == x.Key));
        }


        private List<Guid> ReduceArray(IEnumerable<Guid> keys)
        {
            return keys
                .Distinct()
                .Where(x => x != default)
                .ToList();
        }
    }
}
