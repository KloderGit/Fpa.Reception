using Service.lC.Extensions;
using Service.lC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Repository
{
    public class GenericRepository<TDomen, TDto> : IRepositoryAsync<TDomen, TDto> where TDto : IConvert<TDto>, new() where TDomen : new()
    {
        protected readonly BaseHttpClient http;
        protected readonly string endpoint;

        protected readonly Func<TDto, TDomen> converter = Converter.GetConverter<TDto,TDomen>();

        public GenericRepository(BaseHttpClient httpClient, string endpoint)
        {
            this.http = httpClient;
            this.endpoint = endpoint;
        }
    
        public async Task<IEnumerable<TDomen>> GetAsync()
        {
            var result = Enumerable.Empty<TDomen>();

            var request = await http.Client.GetAsync(endpoint + "/");

            if (request.IsSuccessStatusCode)
            {
                var dto = await request.GetResultAsync<IEnumerable<TDto>>();

                var domain = dto.Select(x => x.ConvertTo<TDomen>(converter));

                result = domain ?? result;
            }

            return result;
        }

        public async Task<TDomen> GetAsync(Guid key)
        {
            TDomen result = default;

            var request = await http.Client.GetAsync(endpoint + "/" + key.ToString());

            if (request.IsSuccessStatusCode)
            {
                var dto = await request.GetResultAsync<TDto>();

                var domain = dto.ConvertTo<TDomen>(converter);

                result = domain ?? result;
            }

            return result;
        }

        public async Task<IEnumerable<TDomen>> GetAsync(IEnumerable<Guid> keys)
        {
            var result = new List<TDomen>();

            if (keys.Count() > 50)
            {
                var array = keys.ToArray();
                var attempt = array.Count();
                var skip = 0;

                while (attempt / 50 >= 1)
                {
                    var partKeys = array.Skip(skip).Take(50).ToList();

                    var partData = await GetResult(partKeys);
                    result.AddRange(partData.ToList());

                    attempt = array.Count() - skip;
                    skip += 50; 
                }
            }
            else
            {
                var res = await GetResult(keys);
                result.AddRange(res.ToList());
            }

            return result;
        }

        private async Task<IEnumerable<TDomen>> GetResult(IEnumerable<Guid> keys)
        {
            var request = await http.Client.GetAsync(endpoint + "/" + "Find", keys);

            var result = Enumerable.Empty<TDomen>();

            if (request.IsSuccessStatusCode)
            {
                var dto = await request.GetResultAsync<IEnumerable<TDto>>();

                var domain = dto.Select(x => x.ConvertTo<TDomen>(converter));

                result = domain ?? result;
            }

            return result;
        }
    }
}