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
                Events = viewModel.Events?.Adapt<IEnumerable<Event>>().ToList(),
                PositionManager = new PositionManager
                {
                    LimitType = viewModel.Type,
                    Positions = viewModel.Times?.Select(t =>
                        new Position { Key = Guid.NewGuid(), IsActive = true, Time = t }
                    ).ToList()
                }
            };

            return reception;
        }

        public static GetReceptionViewModel ConvertDomainViewModel(Domain.Reception reception)
        {
            var viewmodel = new GetReceptionViewModel
            {
                Key = reception.Key,
                IsActive = reception.IsActive,
                Date = reception.Date,
                Type = reception.PositionManager.LimitType,
                Events = reception.Events.Adapt<IEnumerable<EventViewModel>>(),
                Positions = reception.PositionManager?.Positions?.Select(x =>
                   new PositionViewModel
                   {
                       Key = x.Key,
                       IsActive = x.IsActive,
                       Time = x.Time,
                       Record = GetRecord(x.Record)
                   }
                 )
            };

            return viewmodel;

            RecordViewModel GetRecord(Domain.Record record)
            {
                if (record == null) return null;

                return new RecordViewModel
                {
                    StudentKey = record.StudentKey,
                    DisciplineKey = record.DisciplineKey,
                    ProgramKey = record.ProgramKey,
                    Result = GetResult(record.Result)
                };
            }

            ResultViewModel GetResult(Domain.Result result)
            {
                if (result == null) return null;

                return new ResultViewModel
                {
                    TeacherKey = result.TeacherKey,
                    ScoreType = result.Score.Type,
                    ScoreValue = result.Score.Value.Item2.ToString(),
                    Comment = result.Comment
                };
            }

        }



    }
}
