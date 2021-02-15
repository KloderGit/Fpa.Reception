using Application.ReceptionComponent;
using Application.ReceptionComponent.Converter;
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
        IMongoRepository<Service.MongoDB.Model.Reception> database;

        ReceptionComponent receptionComponent;

        public ReceptionController(IMongoRepository<Service.MongoDB.Model.Reception> mongodb)
        {
            this.database = mongodb;
            this.receptionComponent = new ReceptionComponent(database);
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = receptionComponent.GetAll();

            var domain = result.Select(x => new Domain.Reception().ConvertFromType(ReceptionConverter.ConvertMongoToDomain, x));

            var viewmodel = domain.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

            return Ok(viewmodel);
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

            var domain = result.Select(x => new Domain.Reception().ConvertFromType(ReceptionConverter.ConvertMongoToDomain, x));

            var viewmodel = domain.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

            return Ok(viewmodel);
        }

        [HttpGet]
        [Route("FindByTeacher")]
        public async Task<ActionResult> FindByTeacher(Guid key)
        {
            var result = receptionComponent.GetReceptionByTeacherKey(key);

            return Ok(result.Adapt<IEnumerable<Domain.Reception>>());
        }




        public Domain.Reception CreateReception()
        {
            var item = new Domain.Reception
            {
                Date = DateTime.Now,
                IsActive = true,
                Key = Guid.NewGuid(),
                Histories = new List<Domain.History> { new Domain.History { Object = Guid.NewGuid(), Action = " создал ", Subject = Guid.NewGuid(), DateTime = DateTime.UtcNow } },
                Events = new List<Domain.Event> {
                         new Domain.Event {
                          Teachers = new List<Domain.BaseInfo>
                          {
                              new Domain.BaseInfo
                              {
                                  Key = Guid.NewGuid(),
                                  Title = "Меркурьев"
                              },
                              new Domain.BaseInfo
                              {
                                  Key = Guid.NewGuid(),
                                  Title = "Калашников"
                              }
                          },
                          Discipline =  new Domain.BaseInfo { Key = Guid.NewGuid(), Title = "Anatomy" },
                             Restrictions = new List<Domain.PayloadRestriction>{
                                 new Domain.PayloadRestriction
                                 {
                                     Program = Guid.NewGuid(),
                                     Group = Guid.NewGuid(),
                                     SubGroup = Guid.NewGuid(),
                                     Option = new Domain.PayloadOption{
                                        CheckAttemps = true,
                                        CheckContractExpired = true,
                                        CheckDependings = false
                                    }
                                }
                             },
                            Requirement = new Domain.PayloadRequirement
                            {
                                AllowedAttemptCount = 10,
                                SubscribeBefore = DateTime.UtcNow,
                                UnsubscribeBefore = DateTime.Now,
                                DependsOnOtherDisciplines = new List<Guid>{ Guid.NewGuid() }
                            }
                         }
                     },
                PositionManager = new Domain.PositionManager()
                {
                    Positions = new List<Domain.Position>
                    {
                        new Domain.Position{
                        Key = Guid.NewGuid(),
                        IsActive = true,
                        Time = DateTime.Now,
                        Record = new Domain.Record
                        {
                            DisciplineKey = Guid.NewGuid(),
                            ProgramKey = Guid.NewGuid(),
                            StudentKey = Guid.NewGuid(),
                            Result = new Domain.Result
                            {
                                TeacherKey = Guid.NewGuid(),
                                Comment = "Rate comment",
                                Score = new Hundred(45)
                            }
                        }
                   }
                  }
                }
            };


            return item;

        }
    }

}
