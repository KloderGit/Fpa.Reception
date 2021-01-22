using Application.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.HttpClient;

namespace Application.Employee
{
    public class EmployeeMethods
    {
        EmployeeHttpClient http;

        public EmployeeMethods(EmployeeHttpClient client)
        {
            http = client;
        }

        public async Task<IEnumerable<EmployeeDto>> GetByPersonKey(IEnumerable<Guid> keys)
        {
            return await http.GetByPersonKey(keys);
        }

        public async Task<IEnumerable<EmployeeDisciplineDto>> GetDisciplines(IEnumerable<Guid> keys)
        {
            return await http.GetDisciplines(keys);
        }
    }
}
