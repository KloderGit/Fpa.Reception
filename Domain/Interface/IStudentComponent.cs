using Domain.Education;
using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IStudentComponent
    {
        Task<IEnumerable<Reception>> GetAttestation(Guid studentKey, Guid programKey);
        Task<IEnumerable<Student>> GetByKeys(IEnumerable<Guid> studentKeys);
        Task<IEnumerable<Contract>> GetContracts(Guid studentKey);
    }
}
