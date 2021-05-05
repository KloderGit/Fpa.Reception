using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Reception.Converter;
using reception.fitnesspro.ru.Controllers.Reception.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Reception
{
    [Route("[controller]")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly IAppContext context;

        public ReceptionController(IAppContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public ActionResult Get()
        {
            var result = context.Reception.Get();

            if (result == default) return NoContent();

            var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

            return Ok(viewmodel);
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

            var result = context.Reception.GetByKey(receptionKey);

            if (result == default) return NoContent();

            var viewmodel = ReceptionViewModelConverter.ConvertDomainViewModel(result);

            return Ok(viewmodel);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreateReception(CreateReceptionViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            var item = new Domain.Reception().ConvertFromType(ReceptionViewModelConverter.ConvertViewModelToDomain, model);

            context.Reception.Create(item);

            return Ok();
        }


        #region OLD implementation

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Post(CreateReceptionViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            var item = new Domain.Reception().ConvertFromType(ReceptionViewModelConverter.ConvertViewModelToDomain, model);

            context.Reception.Create(item);

            return Ok();
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

            var result = await context.Reception.GetByDisciplineKey(key);

            if (result == default) return NoContent();

            var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

            return Ok(viewmodel);
        }

<<<<<<< HEAD
        [HttpGet]
        [Obsolete]
        public ActionResult Get()
        {
            var result = context.Reception.Get();

            if (result == default) return NoContent();

            var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

            return Ok(viewmodel);
        }

=======
>>>>>>> master
        #endregion
    }

}
