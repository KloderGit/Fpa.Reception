using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IReceptionComponent
    {
        Task<Reception> Create(Reception reception);
        Task Update(Reception reception);
        void Delete(Guid key);

        IEnumerable<Reception> Get();
        Reception GetByKey(Guid key);
        Task<Reception> GetByPosition(Guid positionKey);

        Task<IEnumerable<Reception>> GetForPeriod(Guid? teacherKey, Guid? disciplineKey, DateTime startAfter, DateTime endBefore);

        [Obsolete]
        Task<IEnumerable<Reception>> GetByDisciplineKey(Guid discilineKey);
    }
}
