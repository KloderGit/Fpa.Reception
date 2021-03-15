using Domain.Education;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ITeacherComponent
    {
        Task<IEnumerable<Program>> GetProgram(Guid teacherKey);
    }
}
