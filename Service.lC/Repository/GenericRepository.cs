using Service.lC.Extensions;
using Service.lC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Repository
{
    public class GenericRepository<T> : IRepositoryAsync<T>
    {
        private readonly BaseHttpClient http;
        private readonly string endpoint;

        public GenericRepository(BaseHttpClient httpClient, string endpoint)
        {
            this.http = httpClient;
            this.endpoint = endpoint;
        }
    
        public async Task<IEnumerable<T>> GetAsync()
        {
            var result = Enumerable.Empty<T>();

            var request = await http.Client.GetAsync(endpoint + "/");

            if (request.IsSuccessStatusCode)
            {
                var query = await request.GetResultAsync<IEnumerable<T>>();
                result = query ?? result;
            }

            return result.ToList();
        }

        public async Task<T> GetAsync(Guid key)
        {
            T result = default;

            var request = await http.Client.GetAsync(endpoint + "/", key.ToString());

            if (request.IsSuccessStatusCode)
            {
                var query = await request.GetResultAsync<T>();
                result = query ?? result;
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetAsync(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<T>();

            var request = await http.Client.GetAsync(endpoint + "/" + "Find", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<T>>();
            }

            return result.ToList();
        }
    }
}