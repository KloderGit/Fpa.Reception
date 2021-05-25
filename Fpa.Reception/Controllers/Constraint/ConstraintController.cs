using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using reception.fitnesspro.ru.Misc;

namespace reception.fitnesspro.ru.Controllers.Constraint
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResourseLoggingFilter))]
    [TypeFilter(typeof(LoggedResultFilterAttribute))]
    [ApiController]
    public class ConstraintController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;

        public ConstraintController(IAppContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }

        [HttpGet]
        [Route("GetByKeys")]
        public async Task<ActionResult<IEnumerable<Domain.Constraint>>> GetByKeys(IEnumerable<Guid> constraintKeys)
        {
            if (constraintKeys == default)
            {
                ModelState.AddModelError(nameof(constraintKeys), "Ключи запроса не указаны");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Setting.Get(constraintKeys);

                if (result == default) return NoContent();

                return Ok(result.ToList());
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Domain.Constraint>>> GetAll()
        {
            try
            {
                var result = context.Setting.GetAll();

                if (result == default) return NoContent();

                return Ok(result.ToList());
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }
        //
        // [HttpPost]
        // [Route("Add")]
        // public async Task<ActionResult> AddConstraint(Domain.Constraint constraint)
        // {
        //     if (constraint.Validate() != true) return BadRequest("Для ограничения не указана дисциплина");
        //
        //     try
        //     {
        //         context.Constraint.Store(constraint);
        //
        //         return Ok();
        //     }
        //     catch (Exception e)
        //     {
        //         logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
        //         return new StatusCodeResult(500);
        //     }
        // }
    }
}
