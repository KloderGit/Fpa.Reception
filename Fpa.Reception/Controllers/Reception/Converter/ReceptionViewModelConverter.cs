using reception.fitnesspro.ru.Controllers.Reception.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace reception.fitnesspro.ru.Controllers.Reception.Converter
{
    public static class ReceptionViewModelConverter
    {
        public static Domain.Reception CreatedViewModelToDomain(CreateReceptionViewModel viewModel)
        {
            var reception = new Domain.Reception
            {
                Date = viewModel.Date,
                IsActive = true,
                Key = Guid.NewGuid(),
                Events = viewModel.Events?.Select(e =>
                    new ReceptionPayload
                    {
                        Teachers = e.Teachers.Select(t =>
                           new BaseInfo { Key = t.Key, Title = t.Title }
                         ).ToList(),
                        Discipline = new BaseInfo { Key = e.Discipline.Key, Title = e.Discipline.Title },
                        Requirement = new PayloadRequirement
                        {
                            AllowedAttempCount = e.Requirement.AllowedAttemptCount,
                            DependsOnOtherDiscipline = e.Requirement.DependsOnOtherDisciplines,
                            SubscribeBefore = e.Requirement.SubscribeBefore,
                            UnsubscribeBefore = e.Requirement.UnsubscribeBefore
                        },
                        Constraints = e.Constraints.Select(c =>
                           new PayloadConstraints
                           {
                               Program = c.Program,
                               Group = c.Group,
                               SubGroup = c.SubGroup,
                               Options = new PayloadOptions
                               {
                                   CheckAttemps = c.Options.CheckAttemps,
                                   CheckContractExpired = c.Options.CheckContractExpired,
                                   CheckDependings = c.Options.CheckDependencies
                               }
                           }
                         ).ToList()
                    }
                ).ToList(),
                PositionManager = new PositionManager
                {
                    LimitType = viewModel.PositionType,
                    Positions = viewModel?.Times.Select(t =>
                        new Position { Key = Guid.NewGuid(), IsActive = true, Time = t }
                    ).ToList()
                }
            };

            return reception;
        }
    }
}
