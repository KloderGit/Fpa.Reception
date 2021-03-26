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
    }
}
