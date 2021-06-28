using Domain.Education;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IPersonComponent
    {
        Task<IEnumerable<Person>> GetByStudent(IEnumerable<Guid> studentsKeys);
        Task<IEnumerable<Person>> GetInfo(IEnumerable<Guid> personKeys);
        Task<IEnumerable<Domain.Education.Person>> FindByQuery(string queryString);
        Task<IEnumerable<Person>> FindByContacts(IEnumerable<string> phones, IEnumerable<string> emails);
    }
}
