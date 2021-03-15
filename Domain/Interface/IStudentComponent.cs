using Domain.Education;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IStudentComponent
    {
        Task<Program> GetEducationByContract(Guid contractKey);
    }
}
