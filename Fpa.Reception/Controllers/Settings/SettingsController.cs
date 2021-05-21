using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using reception.fitnesspro.ru.Misc;

namespace reception.fitnesspro.ru.Controllers.Settings
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResourseLoggingFilter))]
    [TypeFilter(typeof(LoggedResultFilterAttribute))]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;

        public SettingsController(IAppContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }
        
        [HttpPost]
        [Route("Disciplines")]
        public async Task<ActionResult> AddDisciplineSettings(BaseConstraint model)
        {
            if (model.Validate() != true) return BadRequest("Для ограничения не указана дисциплина");

            try
            {
                var result = context.Constraint.Store(model);

                return Ok(result.ToString());
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }
        
        [HttpPost]
        [Route("Disciplines/Update")]
        public async Task<ActionResult> EditDisciplineSettings(BaseConstraint model)
        {
            if (model.Key == default) return BadRequest("Не указан ключ");

            try
            {
                context.Constraint.Update(model);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }
        
        [HttpGet]
        [Route("Disciplines")]
        public async Task<ActionResult<IEnumerable<Domain.BaseConstraint>>> GetAllDisciplineSettings()
        {
            try
            {
                var result = context.Constraint.GetAll();

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
        [Route("Disciplines/GetByKeys")]
        public async Task<ActionResult<IEnumerable<Domain.BaseConstraint>>> GetDisciplineSettingsByKeys(IEnumerable<Guid> constraintKeys)
        {
            if (constraintKeys == default)
            {
                ModelState.AddModelError(nameof(constraintKeys), "Ключи запроса не указаны");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Constraint.Get(constraintKeys);

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
        [Route("Disciplines/GetByKey")]
        public async Task<ActionResult<Domain.BaseConstraint>> GetDisciplineSettingsByKey(Guid key)
        {
            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указаны");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Constraint.GetByKey(key);

                if (result == default) return NoContent();

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }
        
        [HttpGet]
        [Route("Disciplines/Find")]
        public async Task<ActionResult<Domain.BaseConstraint>> FindDisciplineSettings(Guid? programKey, Guid disciplineKey)
        {
            if (disciplineKey == default)
            {
                ModelState.AddModelError(nameof(disciplineKey), "Ключ дисциплины не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Constraint.Find(programKey, disciplineKey);

                if (result == default) return NoContent();

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }
    }

}