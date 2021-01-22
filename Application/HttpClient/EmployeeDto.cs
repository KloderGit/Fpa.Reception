using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Application.HttpClient
{
    public class EmployeeDto
    {
        [JsonProperty("key")]
        public Guid Key { get; set; }
        [JsonProperty("personKey")]
        public Guid PersonKey { get; set; }
    }

    public class EmployeeAssignmentDto
    {
        [JsonProperty("employees")]
        public List<EmployeeDisciplineDto> Employees { get; } = new List<EmployeeDisciplineDto>();
    }

    public class EmployeeDisciplineDto
    {
        [JsonProperty("employeeKey")]
        public Guid TeacherKey { get; set; }
        [JsonProperty("disciplines")]
        public IEnumerable<Guid> Disciplines { get; set; }
    }

}
