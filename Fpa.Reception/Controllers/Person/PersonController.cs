using Application.Employee;
using Application.Person;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.HttpClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using Domain.Interface;
using Service.lC;
using Application.Component;
using Microsoft.Extensions.Logging;
using reception.fitnesspro.ru.Misc;

namespace reception.fitnesspro.ru.Controllers.Person
{
    /// <summary>
    /// Физ лицо 1С
    /// </summary>
    [Route("[controller]")]
    [TypeFilter(typeof(ResourseLoggingFilter))]
    [TypeFilter(typeof(LoggedResultFilterAttribute))]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;
        private readonly IdentityHttpClient identityHttpClient;
        private readonly AssignHttpClient assignHttpClient;
        private readonly PersonMethods personAction;
        private readonly EmployeeMethods employeeAction;

        public PersonController(
            IAppContext context, ILoggerFactory loggerFactory,
            
            PersonHttpClient personHttpClient, 
            EmployeeHttpClient employeeHttpClient,
            IdentityHttpClient identityHttpClient,
            AssignHttpClient assignHttpClient)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());


            this.identityHttpClient = identityHttpClient;
            this.assignHttpClient = assignHttpClient;
            personAction = new PersonMethods(personHttpClient);
            employeeAction = new EmployeeMethods(employeeHttpClient,assignHttpClient);
        }

        [HttpGet]
        [Route("GetByToken")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetByToken()
        {
            try
            {
                var bearerToken = Request.Headers[HeaderNames.Authorization];
                var token = bearerToken.ToString().Replace("Bearer ", "");

                if (token == default) return BadRequest("Token is null");

                var user = await identityHttpClient.GetUserInfo(token);

                if (String.IsNullOrEmpty(user?.Email) && String.IsNullOrEmpty(user?.Phone)) return NotFound("Не заполнены контакты пользователя");

                var personGuidArray = await personAction.GetByContacts(new List<string>{user.Phone}, new List<string>{user.Email} );

                if(personGuidArray.IsNullOrEmpty()) return NoContent();

                var employees = await employeeAction.GetByPersonKey(personGuidArray);

                var viewModel = personGuidArray.Select(p=> new PersonViewModel{ 
                    PersonKey = p, 
                    EmployeeKey = employees.IsNullOrEmpty() ? Guid.Empty :
                        employees.Any(x=>x.PersonKey == p) ? employees.First(x=>x.PersonKey == p).Key : Guid.Empty
                });

                return viewModel.ToList();
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
            
        }

        [HttpPost]
        [Route("Info")]
        public async Task<ActionResult<IEnumerable<Domain.Education.Person>>> GetInfo([FromBody]IEnumerable<Guid> keys)
        {
            try
            {
                var persons = await context.Person.GetInfo(keys);

                return persons.ToList();
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

        }

    }


}
