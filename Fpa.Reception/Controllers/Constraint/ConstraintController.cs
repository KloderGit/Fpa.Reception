using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace reception.fitnesspro.ru.Controllers.Constraint
{
    [Route("[controller]")]
    [ApiController]
    public class ConstraintController : ControllerBase
    {
        private readonly IAppContext context;

        public ConstraintController(IAppContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("GetByKeys")]
        public async Task<ActionResult<IEnumerable<Domain.Constraint>>> GetByKeys(IEnumerable<Guid> constraintKeys)
        {
            if (constraintKeys == default) return BadRequest();

            var result = context.Constraint.Get(constraintKeys);

            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Domain.Constraint>>> GetByKey(IEnumerable<Guid> constraintKeys)
        {
            if (constraintKeys == default) return BadRequest();

            var result = context.Constraint.GetAll();

            return Ok(result.ToList());
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> AddConstraint(Domain.Constraint constraint)
        {
            if (constraint.Validate() != true) return BadRequest("Для ограничения не указана дисциплина");

            context.Constraint.Store(constraint);

            return Ok();
        }
    }
}
