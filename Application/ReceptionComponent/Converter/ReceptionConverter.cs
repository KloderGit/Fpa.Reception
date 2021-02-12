using Domain;
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
        public static Service.MongoDB.Model.Reception ConvertToMongoDto(Domain.Reception reception)
        {
            var item = new Service.MongoDB.Model.Reception
            {
                Date = reception.Date,
                IsActive = reception.IsActive,
                Key = reception.Key,
                Histories = reception.Histories?.Adapt<IEnumerable<Service.MongoDB.Model.History>>(),
                Events = reception.Events?.Select(x =>
                    new Service.MongoDB.Model.Event
                    {
                        Teachers = x.Teachers?.Adapt<IEnumerable<Service.MongoDB.Model.BaseInfo>>(),
                        Discipline = x.Discipline?.Adapt<Service.MongoDB.Model.BaseInfo>(),
                        Restrictions = x.Restrictions?.Adapt<IEnumerable<Service.MongoDB.Model.Restriction>>(),
                        Requirement = x.Requirement?.Adapt<Service.MongoDB.Model.Requirement>()
                    }),
                PositionManager = GetPositionManager(reception.PositionManager)
            };

            Service.MongoDB.Model.PositionManager GetPositionManager(Domain.PositionManager manager)
            {
                if (manager == null) return null;

                return new Service.MongoDB.Model.PositionManager
                {
                    LimitType = (PositionTypeDto)((int)reception.PositionManager.LimitType),
                    Positions = manager.Positions?.Select(p =>
                      new Service.MongoDB.Model.Position
                      {
                          Key = p.Key,
                          Time = p.Time,
                          IsActive = p.IsActive,
                          Histories = p.Histories.Select(h => new Service.MongoDB.Model.History { Action = h.Action, DateTime = h.DateTime, Object = h.Object, Subject = h.Subject }),
                          Record = GetRecord(p.Record)
                      })
                };
            }

            Service.MongoDB.Model.Record GetRecord(Domain.Record record)
            {
                if (record == null) return null;

                return new Service.MongoDB.Model.Record
                {
                    StudentKey = record.StudentKey,
                    DisciplineKey = record.DisciplineKey,
                    ProgramKey = record.ProgramKey,
                    Result = GetResult(record.Result)
                };
            }

            Service.MongoDB.Model.Result GetResult(Domain.Result result)
            {
                if (result == null) return null;

                return new Service.MongoDB.Model.Result
                {
                    TeacherKey = result.TeacherKey,
                    Score = new Service.MongoDB.Model.Score { Type = (Service.MongoDB.Model.ScoreType)(int)result.Score.Type, Value = new Tuple<string, object>(result.Score.Value.Item1.FullName, result.Score.Value.Item2) },
                    Comment = result.Comment
                };
            }

            return reception.Adapt<Service.MongoDB.Model.Reception>();
        }

        public static Domain.Reception ConvertMongoToDomain(Service.MongoDB.Model.Reception dto)
        {
            var item = new Domain.Reception
            {
                Date = dto.Date,
                IsActive = dto.IsActive,
                Key = dto.Key,
                Histories = dto.Histories?.Adapt<IEnumerable<Domain.History>>().ToList(),
                Events = dto.Events?.Select(x =>
                    new Domain.Event
                    {
                        Teachers = x.Teachers?.Adapt<IEnumerable<Domain.BaseInfo>>().ToList(),
                        Discipline = x.Discipline?.Adapt<Domain.BaseInfo>(),
                        Restrictions = x.Restrictions?.Adapt<IEnumerable<Domain.PayloadRestriction>>().ToList(),
                        Requirement = x.Requirement?.Adapt<Domain.PayloadRequirement>()
                    }).ToList(),
                PositionManager = GetPositionManager(dto.PositionManager)
            };

            Domain.PositionManager GetPositionManager(Service.MongoDB.Model.PositionManager manager)
            {
                if (manager == null) return null;

                return new Domain.PositionManager
                {
                    LimitType = (PositionType)((int)manager.LimitType),
                    Positions = manager.Positions?.Select(p =>
                      new Domain.Position
                      {
                          Key = p.Key,
                          Time = p.Time,
                          IsActive = p.IsActive,
                          Histories = p.Histories.Select(h => new Domain.History { Action = h.Action, DateTime = h.DateTime, Object = h.Object, Subject = h.Subject }).ToList(),
                          Record = GetRecord(p.Record)
                      }).ToList()
                };
            }

            Domain.Record GetRecord(Service.MongoDB.Model.Record record)
            {
                if (record == null) return null;

                return new Domain.Record
                {
                    StudentKey = record.StudentKey,
                    DisciplineKey = record.DisciplineKey,
                    ProgramKey = record.ProgramKey,
                    Result = GetResult(record.Result)
                };
            }

            Domain.Result GetResult(Service.MongoDB.Model.Result result)
            {
                if (result == null) return null;

                return new Domain.Result
                {
                    TeacherKey = result.TeacherKey,
                    Score = GetScore(result.Score),
                    Comment = result.Comment
                };
            }

            Domain.Score GetScore(Service.MongoDB.Model.Score score)
            {
                if (score == null) return null;

                return new Domain.Score
                {
                    Type = (Domain.ScoreType)(int)score.Type,
                    Value = new Tuple<Type, object>(Type.GetType(score.Value.Item1), score.Value.Item2)
                };
            }


            return item;
        }
    }
}
