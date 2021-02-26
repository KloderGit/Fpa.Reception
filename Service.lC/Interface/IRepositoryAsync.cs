using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IRepositoryAsync<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsync(Guid key);
        Task<IEnumerable<T>> GetAsync(IEnumerable<Guid> keys);
    }
}