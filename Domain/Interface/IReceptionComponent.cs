using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IReceptionComponent
    {
        void Create(Reception reception);
        Task Update(Reception reception);
        void Delete(Guid key);

        IEnumerable<Reception> Get();
        Reception GetByKey(Guid key);
        Task<Reception> GetByPosition(Guid positionKey);

        [Obsolete]
        Task<IEnumerable<Reception>> GetByDisciplineKey(Guid discilineKey);
    }
}
