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

namespace reception.fitnesspro.ru.Controllers.Person
{
    /// <summary>
    /// Физ лицо 1С
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IdentityHttpClient identityHttpClient;
        private readonly PersonMethods personAction;
        private readonly EmployeeMethods employeeAction;

        public PersonController(
            PersonHttpClient personHttpClient, 
            EmployeeHttpClient employeeHttpClient,
            IdentityHttpClient identityHttpClient)
        {
            this.identityHttpClient = identityHttpClient;
            personAction = new PersonMethods(personHttpClient);
            employeeAction = new EmployeeMethods(employeeHttpClient);
        }

        [HttpGet]
        [Route("GetByToken")]
        public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetByToken()
        {
            var bearerToken = Request.Headers[HeaderNames.Authorization];
            var token = bearerToken.ToString().Replace("Bearer ", "");

            var user = await identityHttpClient.GetUserInfo(token);

            if (String.IsNullOrEmpty(user?.Email) && String.IsNullOrEmpty(user?.Phone)) return NotFound("Не заполнены контакты пользователя");

            var personGuidArray = await personAction.GetByContacts(new List<string>{user.Phone}, new List<string>{user.Email} );

            if(personGuidArray.IsNullOrEmpty()) return NoContent();

            var employees = await employeeAction.GetByPersonKey(personGuidArray);

            var viewModel = personGuidArray.Select(p=> new PersonViewModel{ 
                PersonKey = p, 
                EmployeeKey = employees.Any(x=>x.PersonKey == p) ? employees.First(x=>x.PersonKey == p).Key : Guid.Empty
            });

            return viewModel.ToList();
        }

    }


}
