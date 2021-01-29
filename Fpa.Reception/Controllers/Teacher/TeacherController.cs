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

namespace reception.fitnesspro.ru.Controllers.Teacher
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private EmployeeHttpClient employeeHttpClient;
        private ProgramHttpClient programHttpClient;
        private readonly AssignHttpClient assignHttpClient;
        private readonly DisciplineHttpClient disciplineHttpClient;
        private readonly EducationFormHttpClient educationFormHttpClient;
        private readonly ControlTypeHttpClient controlTypeHttpClient;

        ProgramMethods programAction;
        EmployeeMethods employeeAction;

        public TeacherController(
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

        /// <summary>
        /// Get teacher info by key
        /// </summary>
        /// <param name="keys">Method takes an array of teachers guids</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByKeys")]
        public async Task<ActionResult<dynamic>> GetByKeys([FromBody] IEnumerable<Guid> keys)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(@"http://localhost:6400/");

            var request = await client.GetAsync("/Employee/GetByKeys", keys).ConfigureAwait(false);

            var result = await request.Content.ReadAsStringAsync();

            return result;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<dynamic>> GetAll()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(@"http://localhost:6400/");

            var request = await client.GetAsync("/Employee/GetAll").ConfigureAwait(false);

            var result = await request.Content.ReadAsStringAsync();

            return result;
        }

        /// <summary>
        /// Get a teacher for certain person by a guid of person
        /// </summary>
        /// <param name="keys">Method takes an array of persons guids</param>
        /// <returns>Ienumerable string</returns>
        [HttpGet]
        [Route("GetByPersonKeys")]
        [Produces("application/json")]

        public async Task<ActionResult<dynamic>> GetByPersonKey([FromBody] IEnumerable<Guid> keys)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(@"http://localhost:6400/");

            var request = await client.GetAsync("/Employee/GetByPersonKeys", keys).ConfigureAwait(false);

            var result = await request.Content.ReadAsStringAsync();

            return result;
        }

        ///// <summary>
        ///// Get an information which program and discipline teacher involved in
        ///// </summary>
        ///// <param name="keys">Method takes an array of emplyee guids</param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("Disciplines")]
        //public async Task<ActionResult<GetDisciplinesViewModel>> GetDisciplines(IEnumerable<Guid> keys)
        //{
        //    var orders = await employeeAction.GetDisciplines(keys);

        //    var programs = await programAction.GetByDiscipline(orders.SelectMany(x=>x.Disciplines));

        //    var viewModel = new GetDisciplinesViewModel(orders, programs).Create();

        //    return viewModel;
        //}


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
