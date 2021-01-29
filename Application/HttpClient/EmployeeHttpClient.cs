using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Employee;
using Application.Extensions;
using Newtonsoft.Json;

namespace Application.HttpClient
{
    public class EmployeeHttpClient : CommonHttpClient
    {
        public EmployeeHttpClient(System.Net.Http.HttpClient client)
        :base(client: client)
        {}

        
        public async Task<IEnumerable<BaseInfoDto>> GetByKeys(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<BaseInfoDto>();

            var request = await Client.GetAsync("Find", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<BaseInfoDto>>();
            }

            return result;
        }

        public async Task<IEnumerable<EmployeeDto>> GetByPersonKey(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<EmployeeDto>();

            var request = await Client.GetAsync("FindByPerson", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<EmployeeDto>>();
            }

            return result;
        }

        public async Task<IEnumerable<EmployeeDisciplineDto>> GetDisciplines(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<EmployeeDisciplineDto>();

            var request = await Client.GetAsync("GetDisciplines", keys);

            if (request.IsSuccessStatusCode)
            {
                var content = await request.GetResultAsync<EmployeeAssignmentDto>();
                if(content.Employees.IsFilled()) result = content.Employees;
            }

            return result;
        }
    }
}
