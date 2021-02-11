using Mapster;
using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.ReceptionComponent.Converter
{
    public static class ReceptionConverter
    {
        public static ReceptionDto ConvertToMongoDto(Domain.Reception reception)
        {
            //var item = new ReceptionDto
            //{
            //    Date = reception.Date,
            //    IsActive = reception.IsActive,
            //    Key = reception.Key,
            //    Histories = reception.Histories?.Adapt<IEnumerable<HistoryDto>>(),
            //    Events = reception.Events?.Select(x =>
            //        new ReceptionPayloadDto
            //        {
            //            Teachers = x.Teachers?.Adapt<IEnumerable<BaseInfoDto>>(),
            //            Discipline = x.Discipline?.Adapt<BaseInfoDto>(),
            //            Restrictions = x.Restrictions?.Adapt<IEnumerable<PayloadRestrictionDto>>(),
            //            Requirement = x.Requirement?.Adapt<PayloadRequirementDto>()
            //        }),
            //    PositionManager = reception.PositionManager?.Adapt<PositionManagerDto>()
            //    //PositionManager = new PositionManagerDto
            //    //{
            //    //    LimitType = (PositionTypeDto)((int)reception.PositionManager.LimitType),
            //    //    Positions = reception.PositionManager.Positions.Select(p =>
            //    //      new PositionDto
            //    //      {
            //    //          Key = p.Key,
            //    //          Time = p.Time,
            //    //          IsActive = p.IsActive,
            //    //          Histories = p.Histories.Select(h => new HistoryDto { Action = h.Action, DateTime = h.DateTime, Object = h.Object, Subject = h.Subject }),
            //    //          Payload = new PositionPayloadDto
            //    //          {
            //    //              DisciplineKey = p.Payload.DisciplineKey,
            //    //              ProgramKey = p.Payload.ProgramKey,
            //    //              StudentKey = p.Payload.StudentKey,
            //    //              Result = new PositionPayloadResultDto
            //    //              {
            //    //                  Comment = p.Payload.Result.Comment,
            //    //                  TeacherKey = p.Payload.Result.TeacherKey,
            //    //                  Score = new ScoreDto
            //    //                  {
            //    //                      Type = (ScoreTypeDto)((int)p.Payload.Result.Score.Type),
            //    //                      Value = new Tuple<string, object>(p.Payload.Result.Score.Value.Item1.FullName, p.Payload.Result.Score.Value.Item2)
            //    //                  }
            //    //              }
            //    //          }
            //    //      })
            //    //}
            //};

            return reception.Adapt<ReceptionDto>();
        }
    }
}
