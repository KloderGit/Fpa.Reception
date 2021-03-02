using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.lC.Interface
{
    public interface IRepositoryAsync<TDomen, TDto>
    {
        Task<IEnumerable<TDomen>> GetAsync();
        Task<TDomen> GetAsync(Guid key);
        Task<IEnumerable<TDomen>> GetAsync(IEnumerable<Guid> keys);
    }
}