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
        private readonly BaseHttpClient http;
        private readonly string endpoint;

        private readonly Func<TDto, TDomen> converter = Converter.GetConverter<TDto,TDomen>();

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

            var request = await http.Client.GetAsync(endpoint + "/", key.ToString());

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
            var result = Enumerable.Empty<TDomen>();

            var request = await http.Client.GetAsync(endpoint + "/" + "Find", keys);

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