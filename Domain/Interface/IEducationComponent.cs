using Domain.Education;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEducationComponent
    {
        Task<IEnumerable<BaseInfo>> GetDisciplinesByKeys(IEnumerable<Guid> disciplineKeys);
        Task<IEnumerable<Program>> GetProgramsByDiscipline(Guid disciplineKey);
        Task<IEnumerable<Program>> GetProgramsByKeys(IEnumerable<Guid> programKeys);        

        Task<Program> GetStudentEducation(Guid programKey);
    }
}
