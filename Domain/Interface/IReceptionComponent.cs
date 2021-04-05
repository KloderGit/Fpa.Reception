using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IReceptionComponent
    {
        IEnumerable<Domain.Reception> Get();
        Reception Get(Guid key);
        Task<IEnumerable<Reception>> GetByDisciplineKey(Guid key);
        Task<IEnumerable<Reception>> GetByPosition(Guid key);
        Task<IEnumerable<Reception>> GetByStudentKey(Guid studentKey);
        Task<IEnumerable<Reception>> GetByTeacherKey(Guid key);
        Task<IEnumerable<Reception>> GetProgramReceptions(Guid programKey);
        System.Threading.Tasks.Task<dynamic> GetReceptions(Guid studentKey, Guid programKey);
        Task ReplaceReception(Reception reception);
        void Store(Reception reception);
    }
}
