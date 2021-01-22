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

        ProgramMethods programAction;
        EmployeeMethods employeeAction;

        public TeacherController(EmployeeHttpClient employeeHttpClient, ProgramHttpClient programHttpClient)
        {
            this.employeeHttpClient = employeeHttpClient;
            this.programHttpClient = programHttpClient;
                
            programAction = new ProgramMethods(programHttpClient);
            employeeAction = new EmployeeMethods(employeeHttpClient);
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

        public async Task<ActionResult<dynamic>> GetByPersonKey([FromBody]IEnumerable<Guid> keys)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(@"http://localhost:6400/");

            var request = await client.GetAsync("/Employee/GetByPersonKeys", keys).ConfigureAwait(false);

            var result = await request.Content.ReadAsStringAsync();

            return result;
        }

        /// <summary>
        /// Get an information which program and discipline teacher involved in
        /// </summary>
        /// <param name="keys">Method takes an array of disciplines guids</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Disciplines")]
        public async Task<ActionResult<GetDisciplinesViewModel>> GetDisciplines(IEnumerable<Guid> keys)
        {
            var orders = await employeeAction.GetDisciplines(keys);

            var programs = await programAction.GetByDiscipline(orders.SelectMany(x=>x.Disciplines));

            var viewModel = new GetDisciplinesViewModel(orders, programs).Create();

            return viewModel;
        }
    }
}
