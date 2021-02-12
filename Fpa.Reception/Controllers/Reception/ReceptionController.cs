using Application.ReceptionComponent;
using Domain;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Reception.Converter;
using reception.fitnesspro.ru.Controllers.Reception.ViewModel;
using reception.fitnesspro.ru.ViewModel;
using Service.MongoDB;
using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Reception
{
    [Route("[controller]")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        IMongoRepository<ReceptionDto> database;

        ReceptionComponent receptionComponent;

        public ReceptionController(IMongoRepository<ReceptionDto> mongodb)
        {
            this.database = mongodb;
            this.receptionComponent = new ReceptionComponent(database);
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = receptionComponent.GetAll();

            return Ok(result.Adapt<IEnumerable<Domain.Reception>>());
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateReceptionViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            var item = new Domain.Reception().ConvertFromType(ReceptionViewModelConverter.ConvertViewModelToDomain, model);

            receptionComponent.StoreReception(item);

            return Ok(item);
        }


        [HttpGet]
        [Route("FindByDiscipline")]
        public async Task<ActionResult> FindByDiscipline(Guid key)
        {
            var result = receptionComponent.GetReceptionByDisciplineKey(key);

            return Ok(result.Adapt<IEnumerable<Domain.Reception>>());
        }

        [HttpGet]
        [Route("FindByTeacher")]
        public async Task<ActionResult> FindByTeacher(Guid key)
        {
            var result = receptionComponent.GetReceptionByTeacherKey(key);

            return Ok(result.Adapt<IEnumerable<Domain.Reception>>());
        }
    }

}
