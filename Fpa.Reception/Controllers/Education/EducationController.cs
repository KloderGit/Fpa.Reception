using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Employee;
using Application.HttpClient;
using Application.Program;
using lc.fitnesspro.library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Teacher;
using Service.lC;
using Service.MongoDB;

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
        public async Task<ActionResult<IEnumerable<TeacherAssignViewModel>>> GetPrototype([FromBody] IEnumerable<Guid> keys)
        {
            var techerDisciplineKeys = await employeeAction.GetTeacherDisciplines(keys);


            var programs = await programAction.GetByDiscipline(techerDisciplineKeys.SelectMany(x => x.Children));

            var teacherKeys = techerDisciplineKeys.Select(x => x.Key);

            var teachers = await employeeAction.GetByKeys(teacherKeys);

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

        [HttpGet]
        [Route("Program/FindByEmployee")]
        public async Task<ActionResult<IEnumerable<EducationInfoViewModel>>> FindByEmployee(Guid key)
        {
            var client = new EducationProgram(new Manager("Kloder", "Kaligula2"));

            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            // get programs with teacher
            var teacherProgramKeys = await client.GetProgramGuidByTeacher(key).ConfigureAwait(false);

            var client2 = new EducationProgram(new Manager("Kloder", "Kaligula2"));

            var teacherProgramSource = await client2.Find(teacherProgramKeys).ConfigureAwait(false);

            var teacherPrograms = teacherProgramSource.Select(x =>
                new ProgramDto
                {
                    Key = x.Key,
                    Title = x.Title,
                    EducationFormKey = x.EducationFormKey,
                    Teachers = x.Teachers.Select(x=>x.TeacherKey),
                    Disciplines = x.Disciplines.Select(d => new DisciplineInfo
                    {
                        DisciplineKey = d.DisciplineKey,
                        ControlTypeKey = d.ControlTypeKey
                    })
                }
            );

            // get program disciplines
            var disciplineInfo = await disciplineHttpClient.Find(teacherPrograms.SelectMany(x => x.Disciplines).Select(d=>d.DisciplineKey));

            // get teachers
            var teacherKeys = teacherPrograms.SelectMany(x => x.Teachers);
            var teachers = await employeeAction.GetByKeys(teacherKeys);

            // get controltypes
            var controlTypeKeys = teacherPrograms.SelectMany(x => x.Disciplines).Select(x => x.ControlTypeKey).Where(x => x != default);
            var controlTypes = await controlTypeHttpClient.GetByKeys(controlTypeKeys);

            // get program education forms
            var educationFormKeys = teacherPrograms.Select(x => x.EducationFormKey).Where(e => e != default);
            var educationForms = await educationFormHttpClient.GetByKeys(educationFormKeys);


            var result = teacherPrograms?.Select(x=>
                new EducationInfoViewModel(x)
                    .AddEducation(educationForms)
                    .AddTeachers(teachers)
                    .AddDisciplines(disciplineInfo, controlTypes)
            );


            // get program groups
            // get group subgroup
            // get limits

            return result.ToList();
        }


        [HttpGet]
        [Route("Program/FindSiblings")]
        public async Task<ActionResult<EducationStructureViewModel>> FindProgramsWithDisciplineKey(Guid key)
        {
            var client = new EducationProgram(new Manager("Kloder", "Kaligula2"));

            var programs = await client.FindSiblings(key);
            if (programs == null || programs.Any() == false) return NoContent();

            var groups = await client.FindProgramGroup(programs.Select(x=>x.Key));

            var subGroups = await client.FindSubgroups(groups.Select(x=>x.Key));

            var viewModel = new EducationStructureViewModel(programs, groups, subGroups);

            return viewModel;
        }



        [HttpPost]
        [Route("Limit/Create")]
        public async Task<ActionResult> CreateLimit([FromBody] LimitViewModel model)
        {
            return Ok();
        }


        [HttpGet]
        [Route("Limit")]
        public async Task<ActionResult> Limit()
        {


            return Ok();
        }

        public class LimitViewModel
        {
            public Guid ProgramKey { get; set; }
            public Guid DisciplineKey { get; set; }
            public IEnumerable<Guid> DependsOnOtherDiscipline { get; set; } = new List<Guid>();
            public int AllowedAttempCount { get; set; }
        }

    }
}
