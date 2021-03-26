using Domain.Education;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ITeacherComponent
    {
        Task<IEnumerable<Program>> GetEducation(Guid key);
    }
}
