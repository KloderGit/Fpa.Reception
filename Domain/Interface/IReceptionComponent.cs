using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IReceptionComponent
    {
        Reception Create(Reception reception);
        Reception Update(Reception reception);
        Reception Delete(Guid key);

        IEnumerable<Reception> Get();
        Reception GetByKey(Guid key);
        Reception GetByPosition(Guid positionKey);
        void CreateRecord(Reception reception);
    }
}
