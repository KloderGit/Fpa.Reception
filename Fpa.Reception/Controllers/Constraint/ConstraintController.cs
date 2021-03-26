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

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> AddConstraint(Domain.Constraint constraint)
        {
            context.Constraint.Store(constraint);

            return Ok();
        }
    }
}
