using reception.fitnesspro.ru.Controllers.Reception.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Mapster;

namespace reception.fitnesspro.ru.Controllers.Reception.Converter
{
    public static class ReceptionViewModelConverter
    {
        public static Domain.Reception ConvertViewModelToDomain(CreateReceptionViewModel viewModel)
        {
            var reception = new Domain.Reception
            {
                Date = viewModel.Date,
                IsActive = true,
                Key = Guid.NewGuid(),
                Events = viewModel.Events?.Adapt<IEnumerable<ReceptionPayload>>().ToList(),
                PositionManager = new PositionManager
                {
                    LimitType = viewModel.PositionType,
                    Positions = viewModel.Times?.Select(t =>
                        new Position { Key = Guid.NewGuid(), IsActive = true, Time = t }
                    ).ToList()
                }
            };

            return reception;
        }
    }
}
