using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IReceptionComponent
    {
        IEnumerable<Reception> Get();
        Reception GetByKey(Guid key);
        void CreateRecord(Reception reception);
    }
}
