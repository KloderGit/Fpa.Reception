using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Teacher
{
    public class TeacherAssignmentDto
    {
        [JsonProperty("employees")]
        public List<TeacherDiscipline> Teachers { get; } = new List<TeacherDiscipline>();

        public void Add(TeacherDiscipline item)
        {
            var teacher = Teachers.FirstOrDefault(x => x.TeacherKey == item.TeacherKey);
            if (teacher != null)
            {
                teacher.Disciplines = item.Disciplines.Concat(teacher.Disciplines);
            }
            else
            {
                Teachers.Add(item);
            }
        }
    }

    public class TeacherDiscipline
    {
        [JsonProperty("employeeKey")]
        public Guid TeacherKey { get; set; }
        [JsonProperty("disciplines")]
        public IEnumerable<Guid> Disciplines { get; set; }
    }
}