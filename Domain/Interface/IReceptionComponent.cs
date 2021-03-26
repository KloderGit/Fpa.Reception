using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IReceptionComponent
    {
        IEnumerable<Domain.Reception> Get();
        IEnumerable<Domain.Reception> GetByDisciplineKey(Guid key);
        IEnumerable<Domain.Reception> GetByTeacherKey(Guid key);
        System.Threading.Tasks.Task<dynamic> GetReceptions(Guid studentKey, Guid programKey);
        void Store(Reception reception);
    }
}
