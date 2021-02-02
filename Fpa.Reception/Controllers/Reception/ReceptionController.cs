using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace reception.fitnesspro.ru.Controllers.Reception
{
    [Route("[controller]")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CreateReceptionViewModel>> Get()
        {
            var viewModel = new CreateReceptionViewModel
            {
                Date = DateTime.UtcNow,
                PositionType = PositionType.Seating,
                Positions = new List<CreateReceptionPositionViewModel> {
                       new CreateReceptionPositionViewModel{
                         Time = DateTime.Now
                       },
                       new CreateReceptionPositionViewModel{
                         Time = DateTime.Now
                       },
                },
                Event = new List<CreateEventViewModel> {
                    new CreateEventViewModel{
                            Teachers = new List<BaseInfoViewModel>{
                                new BaseInfoViewModel{ Key = Guid.NewGuid(), Title = "Kalashnikov" },
                                new BaseInfoViewModel{ Key = Guid.NewGuid(), Title = "Merkuriev" },
                             },
                            Discipline = new BaseInfoViewModel { Key = Guid.NewGuid(), Title = "ResultingExam" },
                            Limits = new CreateReceptionLimitViewModel {
                                AllowedAttempCount = 20,
                                DependsOnOtherDiscipline = new List<Guid> { Guid.NewGuid() },
                                SubscribeBefore = DateTime.Now,
                                UnsubscribeBefore = DateTime.Now + TimeSpan.FromDays(2)
                            },
                            Bound = new List<CreateReceptionBoundViewModel>{
                                 new CreateReceptionBoundViewModel{
                                     Program = Guid.NewGuid(),
                                      Group = Guid.NewGuid(),
                                       SubGroup = Guid.NewGuid(),
                                        Rules = new CreateReceptionRulesViewModel()
                                 }
                             },
                      } 
                }
            };

            return viewModel;
        }
    }

    public class BaseInfoViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }

    public class CreateReceptionViewModel
    {
        public DateTime Date { get; set; }

        public IEnumerable<CreateEventViewModel> Event { get; set; }

        public PositionType PositionType { get; set; }

        public IEnumerable<CreateReceptionPositionViewModel> Positions { get; set; }
    }

    public class CreateEventViewModel
    {
        public IEnumerable<BaseInfoViewModel> Teachers { get; set; } = new List<BaseInfoViewModel>();
        public BaseInfoViewModel Discipline { get; set; }
        public IEnumerable<CreateReceptionBoundViewModel> Bound { get; set; } = new List<CreateReceptionBoundViewModel>();
        public CreateReceptionLimitViewModel Limits { get; set; }
    }

    public class CreateReceptionLimitViewModel
    {
        public DateTime SubscribeBefore { get; set; } = default;
        public DateTime UnsubscribeBefore { get; set; } = default;

        public IEnumerable<Guid> DependsOnOtherDiscipline { get; set; } = new List<Guid>();
        public int AllowedAttempCount { get; set; }
    }

    public class CreateReceptionBoundViewModel
    {
        public Guid Program { get; set; }
        public Guid Group { get; set; }
        public Guid SubGroup { get; set; }

        public CreateReceptionRulesViewModel Rules { get; set; }
    }

    public class CreateReceptionRulesViewModel
    {
        public bool CheckContractExpired { get; set; }
        public bool CheckDependings { get; set; }
        public bool CheckAttemps { get; set; }
    }

    public class CreateReceptionPositionViewModel
    {
        public DateTime Time { get; set; }
    }
}
