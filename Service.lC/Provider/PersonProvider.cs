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

        //public async Task<Person> FindByStudent(Guid studentKey)
        //{
        //    if (studentKey == default) throw new ArgumentNullException(nameof(studentKey));

        //    var studentQuery = manager.Student
        //                .Select(x => x.PersonKey)
        //                .Filter(x => x.DeletionMark == false).And()
        //                .Filter(x => x.Key == studentKey);
        //    var foundedStudent = await studentQuery.GetByFilter();

        //    if (foundedStudent.IsNullOrEmpty()) return null;

        //    var personQuery = manager.Person
        //        .Select(x => x.Key)
        //        .Filter(x => x.DeletionMark == false).And()
        //        .Filter(x => x.Key == foundedStudent.FirstOrDefault().Key);
        //    var person = await personQuery.GetByFilter();

        //    var contracts = await Repository.GetAsync(keys);

        //    return contracts;

        //}
    }
}
