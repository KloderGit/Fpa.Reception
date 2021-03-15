using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IPersonComponent
    {
        Task<IEnumerable<Person>> GetInfo(IEnumerable<Guid> personKeys);
    }
}
