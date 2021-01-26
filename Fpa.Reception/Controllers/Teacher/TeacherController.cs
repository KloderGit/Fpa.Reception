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

namespace reception.fitnesspro.ru.Controllers.Teacher
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private EmployeeHttpClient employeeHttpClient;
        private ProgramHttpClient programHttpClient;
        private readonly AssignHttpClient assignHttpClient;

        ProgramMethods programAction;
        EmployeeMethods employeeAction;

        public TeacherController(
            EmployeeHttpClient employeeHttpClient,
            ProgramHttpClient programHttpClient,
            AssignHttpClient assignHttpClient)
        {
            this.employeeHttpClient = employeeHttpClient;
            this.programHttpClient = programHttpClient;
            this.assignHttpClient = assignHttpClient;

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
        [Route("Disciplines")]
        public async Task<ActionResult<IEnumerable<TeacherInfoViewModel>>> GetDisciplines(IEnumerable<Guid> keys)
        {
            var disciplineKeys = await employeeAction.GetTeacherDisciplines(keys);

            var programs = await programAction.GetByDiscipline(disciplineKeys.SelectMany(x => x.Disciplines));

            var vm = keys.Select(x =>
                new TeacherInfoViewModel
                {
                    Key = x,
                    Programs = GetPrograms(disciplineKeys.First(t=>t.TeacherKey==x).Disciplines)
                }
            );

            IEnumerable<ProgramInfoViewModel> GetPrograms(IEnumerable<Guid> keys)
            {
                var pr = keys.SelectMany(x => GetProgram(x));

                var sddd = pr.GroupBy(x => x.Key);

                var res = sddd.Select(x => new ProgramInfoViewModel
                {
                    Key = x.Key,
                    Form = x.First().Form,
                    Disciplines = x.SelectMany(d=>d.Disciplines)
                });

                return res;
            }

            IEnumerable<ProgramInfoViewModel> GetProgram(Guid key)
            {
                var progs = programs.Where(x => 
                    x.Disciplines.Select(k => k.DisciplineKey)
                        .Any(i => i == key));

                var vms = progs.Select(x => new ProgramInfoViewModel
                {
                    Key = x.Key,
                    Form = x.EducationFormKey,
                    Disciplines = x.Disciplines.Where(d=>d.DisciplineKey == key)
                    .Select(d=>new DisciplineInfoViewModel
                    {
                        Key = d.DisciplineKey,
                        ControlTypeKey = d.ControlTypeKey
                    })
                });

                return vms;
            }

            return vm.ToList();
        }

        public class TeacherInfoViewModel
        {
            public Guid Key { get; set; }
            public IEnumerable<ProgramInfoViewModel> Programs { get; set; }
        }

        public class ProgramInfoViewModel
        {
            public Guid Key { get; set; }
            public Guid Form { get; set; }

            public IEnumerable<DisciplineInfoViewModel> Disciplines { get; set; }
        }

        public class DisciplineInfoViewModel
        {
            public Guid Key { get; set; }
            public Guid ControlTypeKey { get; set; }
        }
    }
}
