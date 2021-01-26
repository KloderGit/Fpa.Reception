using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Extensions;
using Newtonsoft.Json;

namespace Application.HttpClient
{
    public class AssignHttpClient : CommonHttpClient
    {
        public AssignHttpClient(System.Net.Http.HttpClient client)
        :base(client: client)
        {}

        public async Task<IEnumerable<AssignDisciplineDto>> GetByTeacherKeys(IEnumerable<Guid> keys)
        {
            var result = Enumerable.Empty<AssignDisciplineDto>();

            var request = await Client.GetAsync("FindByEmployees", keys);

            if (request.IsSuccessStatusCode)
            {
                result = await request.GetResultAsync<IEnumerable<AssignDisciplineDto>>();
            }

            return result.ToList();
        }

        public class AssignDisciplineDto
        {
            public IEnumerable<TeacherForDisciplineDto> Teachers { get; set; }

            public IEnumerable<DisciplineForTeacherDto> Disciplines { get; set; }
        }

        public class TeacherForDisciplineDto
        {
            public Guid TeacherKey { get; set; }
            public int Marker { get; set; }
        }
        public class DisciplineForTeacherDto
        {
            public Guid DisciplineKey { get; set; }
            public int Marker { get; set; }
        }
    }
}
