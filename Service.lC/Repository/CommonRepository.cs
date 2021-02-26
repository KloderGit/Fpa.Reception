using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Extensions;
using Domain.Interface;

namespace Application.Repository
{
    public class CommonRepository<T> : IRepositoryAsync<T>
    {
        private readonly System.Net.Http.HttpClient http;

        public CommonRepository(System.Net.Http.HttpClient httpClient)
        {
            this.http = httpClient;
        }
    
        public async Task<IEnumerable<T>> GetAsync()
        {
            var result = Enumerable.Empty<T>();

            var request = await http.GetAsync("/");

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

            var request = await http.GetAsync("/", key.ToString());

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

            var request = await http.GetAsync("Find", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<T>>();
            }

            return result.ToList();
        }
    }
}