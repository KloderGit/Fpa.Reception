using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Employee;
using Application.HttpClient;
using Application.Program;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Teacher;

namespace reception.fitnesspro.ru.Controllers.Education
{
    [Route("[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private EmployeeHttpClient employeeHttpClient;
        private ProgramHttpClient programHttpClient;
        private readonly AssignHttpClient assignHttpClient;
        private readonly DisciplineHttpClient disciplineHttpClient;
        private readonly EducationFormHttpClient educationFormHttpClient;
        private readonly ControlTypeHttpClient controlTypeHttpClient;

        ProgramMethods programAction;
        EmployeeMethods employeeAction;

        public EducationController(
            EmployeeHttpClient employeeHttpClient,
            ProgramHttpClient programHttpClient,
            AssignHttpClient assignHttpClient,
            DisciplineHttpClient disciplineHttpClient,
            EducationFormHttpClient educationFormHttpClient,
            ControlTypeHttpClient controlTypeHttpClient)
        {
            this.employeeHttpClient = employeeHttpClient;
            this.programHttpClient = programHttpClient;
            this.assignHttpClient = assignHttpClient;
            this.disciplineHttpClient = disciplineHttpClient;
            this.educationFormHttpClient = educationFormHttpClient;
            this.controlTypeHttpClient = controlTypeHttpClient;

            programAction = new ProgramMethods(programHttpClient);
            employeeAction = new EmployeeMethods(employeeHttpClient, assignHttpClient);
        }

        [HttpPost]
        [Route("Prototype")]
        public async Task<ActionResult<IEnumerable<TeacherAssignViewModel>>> GetPrototype([FromBody]IEnumerable<Guid> keys)
        {
            var techerDisciplineKeys = await employeeAction.GetTeacherDisciplines(keys);

            var programs = await programAction.GetByDiscipline(techerDisciplineKeys.SelectMany(x => x.Children));

            var teachers = await employeeAction.GetByKeys(keys);

            var disciplineInfo = await disciplineHttpClient.Find(techerDisciplineKeys.SelectMany(x => x.Children));

            var educationForms = await educationFormHttpClient.GetByKeys(programs.Select(e => e.EducationFormKey).Where(e => e != default));

            var controlTypes = await controlTypeHttpClient.GetByKeys(programs.SelectMany(d => d.Disciplines.Select(i => i.ControlTypeKey)));

            ;

            var res = from info in techerDisciplineKeys
                      let teacher = teachers.FirstOrDefault(x => x.Key == info.Key)
                      let discipline = info.Children.Select(d =>
                          new DisciplineViewModel
                          {
                              Key = d,
                              Title = disciplineInfo.FirstOrDefault(x => x.Key == d).Title
                          })
                      let programT = info.Children
                          .SelectMany(x => programs.Where(p => p.Disciplines.Any(d => d.DisciplineKey == x))
                              .Select(i =>
                                  new Teacher.ProgramInfoViewModel
                                  {
                                      Key = i.Key,
                                      Title = i.Title,
                                      EducationForm = new EducationFormViewModel
                                      {
                                          Key = i.EducationFormKey,
                                          Title = educationForms.FirstOrDefault(e => e.Key == i.EducationFormKey)?.Title
                                      },
                                      Disciplines =
                                          new List<DisciplinePlanViewModel>
                                          {
                                              new DisciplinePlanViewModel
                                              {
                                                  Discipline = discipline.First(k => k.Key == x),
                                                  ControlType =
                                                      new ControlTypeViewModel
                                                      {
                                                          Key = i.Disciplines.FirstOrDefault(ct=>ct.DisciplineKey == x).ControlTypeKey,
                                                          Title = controlTypes.FirstOrDefault(ctp=>ctp.Key == i.Disciplines.FirstOrDefault(ct=>ct.DisciplineKey == x).ControlTypeKey)?.Title
                                                      }
                                              }
                                          }
                                  })
                          ).GroupBy(g => g.Key)
                          .Select(r => new Teacher.ProgramInfoViewModel
                          {
                              Key = r.Key,
                              Title = r.First().Title,
                              EducationForm = r.First().EducationForm,
                              Disciplines = r.SelectMany(d => d.Disciplines)
                          })
                      select new TeacherAssignViewModel
                      {
                          Key = teacher.Key,
                          Title = teacher.Title,
                          Programs = programT
                      };

            return res.ToList();
        }
    }
}
