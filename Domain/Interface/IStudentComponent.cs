using Domain.Education;
using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IStudentComponent
    {
        Task<Program> GetStudentEducation(Guid programKey);
        Task<IEnumerable<Reception>> GetReceptionsForSignUpStudent(Guid studentKey, Guid programKey);
        Task<IEnumerable<Student>> GetStudents(IEnumerable<Guid> studentKeys);
        Task<IEnumerable<Contract>> GetContracts(Guid studentKey);
        Task<IEnumerable<Reception>> GetReceptionsWithSignedUpStudent(Guid studentKey);
    }
}
