using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Reception.Converter;
using reception.fitnesspro.ru.Controllers.Reception.ViewModel;
using reception.fitnesspro.ru.Misc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace reception.fitnesspro.ru.Controllers.Reception
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResourseLoggingFilter))]
    [TypeFilter(typeof(LoggedResultFilterAttribute))]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;

        public ReceptionController(IAppContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }
        
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = context.Reception.Get();

                if (result == default) return NoContent();

                var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

                return Ok(viewmodel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
            
        }

        [HttpGet]
        [Route("GetByKey")]
        public async Task<ActionResult> GetByKey(Guid receptionKey)
        {
            if (receptionKey == default)
            {
                ModelState.AddModelError(nameof(receptionKey), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Reception.GetByKey(receptionKey);

                if (result == default) return NoContent();

                var viewmodel = ReceptionViewModelConverter.ConvertDomainViewModel(result);

                return Ok(viewmodel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreateReception(CreateReceptionViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            try
            {
                var item = new Domain.Reception().ConvertFromType(ReceptionViewModelConverter.ConvertViewModelToDomain, model);

                context.Reception.Create(item);

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }


        #region OLD implementation

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Post(CreateReceptionViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            try
            {
                var item = new Domain.Reception().ConvertFromType(ReceptionViewModelConverter.ConvertViewModelToDomain, model);

                context.Reception.Create(item);

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
         
        }

        [HttpGet]
        [Route("FindByDiscipline")]
        [Obsolete]
        public async Task<ActionResult> FindByDiscipline(Guid key)
        {
            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await context.Reception.GetByDisciplineKey(key);

                if (result == default) return NoContent();

                var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

                return Ok(viewmodel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
           
        }

        [HttpGet]
        [Obsolete]
        public ActionResult Get()
        {
            try
            {
                var result = context.Reception.Get();

                if (result == default) return NoContent();

                var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

                return Ok(viewmodel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        #endregion
    }

}
