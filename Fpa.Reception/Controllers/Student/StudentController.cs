using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IAppContext context;

        public StudentController(IAppContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("Attestation")]
        public async Task<ActionResult> GetAtteststion(Guid studentKey, Guid programKey)
        {
            var studentComponent = context.Student;
            var receptions = await studentComponent.GetAttestation(studentKey, programKey);

            return receptions;
        }
    }
}
