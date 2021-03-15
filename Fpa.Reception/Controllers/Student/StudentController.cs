using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Component;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.lC;

namespace reception.fitnesspro.ru.Controllers.Student
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly Context lcService;

        IStudentComponent studentComponent;

        public StudentController(Context lcService)
        {
            this.lcService = lcService;
            studentComponent = new StudentComponent(lcService);
        }



        [HttpGet]
        [Route("Education")]
        public async Task<ActionResult> GetStudentEducation(Guid key)
        {
            var program = await studentComponent.GetEducationByContract(key);

            return Ok(program);
        }
    }
}
