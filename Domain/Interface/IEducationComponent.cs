using Domain.Education;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEducationComponent
    {
        Task<IEnumerable<Program>> FindByDiscipline(Guid disciplineKey);
        Task<IEnumerable<Program>> GetAllPrograms();
        Task<IEnumerable<BaseInfo>> GetDisciplinesByKeys(IEnumerable<Guid> disciplineKeys);
        Task<IEnumerable<BaseInfo>> GetProgramsByKeys(IEnumerable<Guid> programKeys);
    }
}
