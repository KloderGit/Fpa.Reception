using Domain.Education;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ITeacherComponent
    {
        Task<IEnumerable<Program>> GetEducation(Guid employeeKey);

        Task<IEnumerable<Reception>> GetReceptions(Guid employeeKey, Guid disciplineKey, DateTime fromDate, DateTime toDate);
    }
}
