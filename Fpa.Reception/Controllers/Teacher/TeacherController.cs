using Application.Employee;
using Application.Program;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Extensions;
using Application.HttpClient;
using Domain;
using Domain.Interface;

namespace reception.fitnesspro.ru.Controllers.Teacher
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly IAppContext context;

        private EmployeeHttpClient employeeHttpClient;
        private ProgramHttpClient programHttpClient;
        private readonly AssignHttpClient assignHttpClient;
        private readonly DisciplineHttpClient disciplineHttpClient;
        private readonly EducationFormHttpClient educationFormHttpClient;
        private readonly ControlTypeHttpClient controlTypeHttpClient;

        ProgramMethods programAction;
        EmployeeMethods employeeAction;

        public TeacherController(
            IAppContext context,
            EmployeeHttpClient employeeHttpClient,
            ProgramHttpClient programHttpClient,
            AssignHttpClient assignHttpClient,
            DisciplineHttpClient disciplineHttpClient,
            EducationFormHttpClient educationFormHttpClient,
            ControlTypeHttpClient controlTypeHttpClient)
        {
            this.context = context;
            this.employeeHttpClient = employeeHttpClient;
            this.programHttpClient = programHttpClient;
            this.assignHttpClient = assignHttpClient;
            this.disciplineHttpClient = disciplineHttpClient;
            this.educationFormHttpClient = educationFormHttpClient;
            this.controlTypeHttpClient = controlTypeHttpClient;

            programAction = new ProgramMethods(programHttpClient);
            employeeAction = new EmployeeMethods(employeeHttpClient, assignHttpClient);
        }

        /// <summary>
        /// Get information in which programs and disciplines the teacher is invoved in
        /// </summary>
        /// <param name="key">Method takes a key of employee</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEducation")]
        public async Task<ActionResult<IEnumerable<Domain.Education.Program>>> GetEducation([FromQuery]Guid key)
        {
            var programs = await context.Teacher.GetEducation(key);

            if(programs.IsNullOrEmpty()) programs = await context.Education.GetAllPrograms();

            if (programs.IsNullOrEmpty()) return NotFound();

            return programs.ToList();
        }


        [HttpGet]
        [Route("GetReception")]
        public async Task<dynamic> GetReception([FromQuery] Guid key)
        {
            // Get Reception
            // Get Discipline Title
            // Get Students person Title
            // Get Students Program Title


            var reception = context.Reception.Get(key);
            if (reception == default) return BadRequest(nameof(key));

            var discipline = (await context.Education.GetDisciplinesByKeys(reception.Events.Select(x=>x.Discipline.Key))).ToList();

            var studentsKeys = reception.PositionManager.Positions
                .Where(x => x.Record != default && x.Record.StudentKey != default)
                .Select(x => x.Record.StudentKey);

            var students = (await context.Student.GetByKeys(studentsKeys)).ToList();

            var persons = (await context.Person.GetByStudent(studentsKeys)).ToList();

            var programsKeys = reception.PositionManager.Positions
                .Where(x => x.Record != default && x.Record.StudentKey != default && x.Record.ProgramKey != default)
                .Select(x => x.Record.ProgramKey);

            var programs = (await context.Education.GetProgramsByKeys(programsKeys)).ToList();

            var vm = new {
                Discipline = discipline.FirstOrDefault(),
                Date = reception.Date,
                Position = reception.PositionManager.Positions
                                                    .Where(x=>x != default && x.Record != default)
                                                    .Select(x => GetTimeViewModel(x))
            };

            return vm;

            dynamic GetTimeViewModel(Position position)
            {
                var student = students.FirstOrDefault(x => x.Key == position.Record.StudentKey);
                var person = persons.FirstOrDefault(x => x.Key == student.Owner);

                var program = programs.FirstOrDefault(x => x.Key == position.Record.ProgramKey);

                return new
                {
                    Time = position.Time,
                    Student = new { Title = person.Title, Key = student.Key },
                    Program = new { Title = program.Title, Key = program.Key },
                    PositionKey = position.Key
                };
            }
        }





        [HttpGet]
        //[Route("Prototype")]
        [Route("Disciplines")]
        public async Task<ActionResult<IEnumerable<TeacherAssignViewModel>>> GetDisciplines(IEnumerable<Guid> keys)
        {
            var techerDisciplineKeys = await employeeAction.GetTeacherDisciplines(keys);

            var programs = await programAction.GetByDiscipline(techerDisciplineKeys.SelectMany(x => x.Children));

            var teachers = await employeeAction.GetByKeys(keys);

            var disciplineInfo = await disciplineHttpClient.Find(techerDisciplineKeys.SelectMany(x => x.Children));

            var educationForms = await educationFormHttpClient.GetByKeys(programs.Select(e=>e.EducationFormKey).Where(e=>e != default));

            var controlTypes = await controlTypeHttpClient.GetByKeys(programs.SelectMany(d=>d.Disciplines.Select(i=>i.ControlTypeKey)));

            var res = from info in techerDisciplineKeys
                      let teacher = teachers.FirstOrDefault(x => x.Key == info.Key)
                      let discipline =(
                              from disciplineKey in info.Children
                              let disciplineEntity = disciplineInfo.FirstOrDefault(d=>d.Key == disciplineKey)
                              select new DisciplineViewModel { Key = disciplineKey, Title = disciplineEntity.Title })
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
                                          Title = educationForms.FirstOrDefault(e=>e.Key == i.EducationFormKey)?.Title
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
                          ).GroupBy(g=>g.Key)
                          .Select(r=>new Teacher.ProgramInfoViewModel
                          {
                              Key = r.Key,
                              Title = r.First().Title,
                              EducationForm = r.First().EducationForm,
                              Disciplines = r.SelectMany(d => d.Disciplines)
                          } )
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
