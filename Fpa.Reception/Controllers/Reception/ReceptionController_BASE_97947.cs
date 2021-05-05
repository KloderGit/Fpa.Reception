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
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var result = context.Reception.Get();

            var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

            return Ok(viewmodel);
        }

        [HttpGet]
        [Route("GetByKey")]
        public async Task<ActionResult> GetByKey(Guid rceptionKey)
        {
            var result = context.Reception.Get();

            var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

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
            var result = await context.Reception.GetByDisciplineKey(key);

            var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

            return Ok(viewmodel);
        }

        [HttpGet]
        [Obsolete]
        public ActionResult Get()
        {
            var result = context.Reception.Get();

            var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

            return Ok(viewmodel);
        }

        #endregion
    }

}
