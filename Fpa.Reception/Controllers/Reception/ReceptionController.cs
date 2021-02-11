using Application.ReceptionComponent;
using Domain;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Reception.Converter;
using reception.fitnesspro.ru.Controllers.Reception.ViewModel;
using reception.fitnesspro.ru.ViewModel;
using Service.MongoDB;
using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        public async Task<ActionResult> Post(CreateReceptionViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            var item = new Domain.Reception().ConvertFromType(ReceptionViewModelConverter.ConvertViewModelToDomain, model);

            receptionComponent.StoreReception(item);

            return Ok(item);
        }







        [HttpGet]
        public async Task<ActionResult<CreateReceptionViewModel>> Get()
        {
            var viewModel = new CreateReceptionViewModel
            {
                Date = DateTime.UtcNow,
                PositionType = PositionType.Seating,               
                Times = new List<DateTime> {
                    DateTime.Now,
                    DateTime.Now
                },
                Events = new List<EventViewModel> {
                    new EventViewModel{
                            Teachers = new List<BaseInfoViewModel>{
                                new BaseInfoViewModel{ Key = Guid.NewGuid(), Title = "Kalashnikov" },
                                new BaseInfoViewModel{ Key = Guid.NewGuid(), Title = "Merkuriev" },
                             },
                            Discipline = new BaseInfoViewModel { Key = Guid.NewGuid(), Title = "ResultingExam" },
                            Requirement = new RequirementViewModel {
                                AllowedAttemptCount = 20,
                                DependsOnOtherDisciplines = new List<Guid> { Guid.NewGuid() },
                                SubscribeBefore = DateTime.Now,
                                UnsubscribeBefore = DateTime.Now + TimeSpan.FromDays(2)
                            },
                            Restrictions = new List<RestrictionViewModel>{
                                 new RestrictionViewModel{
                                     Program = Guid.NewGuid(),
                                      Group = Guid.NewGuid(),
                                       SubGroup = Guid.NewGuid(),
                                       Options = new OptionViewModel()
                                 }
                             },
                      } 
                }
            };

            return viewModel;
        }


    }

}
