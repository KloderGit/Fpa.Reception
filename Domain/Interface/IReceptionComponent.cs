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
        IEnumerable<Domain.Reception> GetByDisciplineKey(Guid key);
        IEnumerable<Reception> GetByPosition(Guid key);
        IEnumerable<Reception> GetByStudentKey(Guid studentKey);
        IEnumerable<Domain.Reception> GetByTeacherKey(Guid key);
        Task<IEnumerable<Reception>> GetProgramReceptions(Guid programKey);
        System.Threading.Tasks.Task<dynamic> GetReceptions(Guid studentKey, Guid programKey);
        void ReplaceReception(Reception reception);
        void Store(Reception reception);
    }
}
