using Domain;
using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Program
{
    public static class Convert
    {
        public static ReceptionDto ConvertToMongoDto(Domain.Reception reception)
        {
            var item = new ReceptionDto
            {
                Date = reception.Date,
                IsActive = reception.IsActive,
                Key = reception.Key,
                Histories = reception.Histories.Select(x => new HistoryDto { Action = x.Action, DateTime = x.DateTime, Object = x.Object, Subject = x.Subject }),
                Events = reception.Events?.Select(x =>
                    new ReceptionPayloadDto
                    {
                        Teachers = x.Teachers.Select(t =>
                           new BaseInfoDto { Key = t.Key, Title = t.Title }
                         ),
                        Discipline = new BaseInfoDto { Key = x.Discipline.Key, Title = x.Discipline.Title },
                        Restrictions = x.Restrictions.Select(b =>
                             new PayloadRestrictionDto
                             {
                                 Group = b.Group,
                                 Program = b.Program,
                                 SubGroup = b.SubGroup,
                                 Options = new PayloadOptionDto
                                 {
                                     CheckAttemps = b.Option.CheckAttemps,
                                     CheckContractExpired = b.Option.CheckContractExpired,
                                     CheckDependings = b.Option.CheckDependings
                                 }
                             }),
                        Requirement = new PayloadRequirementDto
                        {
                            AllowedAttempCount = x.Requirement.AllowedAttemptCount,
                            DependsOnOtherDiscipline = x.Requirement.DependsOnOtherDisciplines,
                            SubscribeBefore = x.Requirement.SubscribeBefore,
                            UnsubscribeBefore = x.Requirement.UnsubscribeBefore
                        }
                    }),
                PositionManager = new PositionManagerDto
                {
                    LimitType = (PositionTypeDto)((int)reception.PositionManager.LimitType),
                    Positions = reception.PositionManager.Positions.Select(p =>
                      new PositionDto
                      {
                          Key = p.Key,
                          Time = p.Time,
                          IsActive = p.IsActive,
                          Histories = p.Histories.Select(h => new HistoryDto { Action = h.Action, DateTime = h.DateTime, Object = h.Object, Subject = h.Subject }),
                          Payload = new PositionPayloadDto
                          {
                              DisciplineKey = p.Payload.DisciplineKey,
                              ProgramKey = p.Payload.ProgramKey,
                              StudentKey = p.Payload.StudentKey,
                              Result = new PositionPayloadResultDto
                              {
                                  Comment = p.Payload.Result.Comment,
                                  TeacherKey = p.Payload.Result.TeacherKey,
                                  Score = new ScoreDto
                                  { 
                                      Type = (ScoreTypeDto)((int)p.Payload.Result.Score.Type),
                                      Value = new Tuple<string, object>(p.Payload.Result.Score.Value.Item1.FullName, p.Payload.Result.Score.Value.Item2) 
                                  }
                              }
                          }
                      })
                }
            };

            return item;
        }

        public static Domain.Reception ConvertFromMongoDto(ReceptionDto reception)
        {
            var item = new Domain.Reception
            {
                Date = reception.Date,
                IsActive = reception.IsActive,
                Key = reception.Key,
                Histories = reception.Histories.Select(x => new History { Action = x.Action, DateTime = x.DateTime, Object = x.Object, Subject = x.Subject }).ToList(),
                Events = reception.Events?.Select(x =>
                    new ReceptionPayload
                    {
                        Teachers = x.Teachers.Select(t =>
                           new BaseInfo { Key = t.Key, Title = t.Title }
                         ).ToList(),
                        Discipline = new BaseInfo { Key = x.Discipline.Key, Title = x.Discipline.Title },
                        Restrictions = x.Restrictions.Select(b =>
                             new PayloadRestriction
                             {
                                 Group = b.Group,
                                 Program = b.Program,
                                 SubGroup = b.SubGroup,
                                 Option = new PayloadOption
                                 {
                                     CheckAttemps = b.Options.CheckAttemps,
                                     CheckContractExpired = b.Options.CheckContractExpired,
                                     CheckDependings = b.Options.CheckDependings
                                 }
                             }).ToList(),
                        Requirement = new PayloadRequirement
                        {
                            AllowedAttemptCount = x.Requirement.AllowedAttempCount,
                            DependsOnOtherDisciplines = x.Requirement.DependsOnOtherDiscipline,
                            SubscribeBefore = x.Requirement.SubscribeBefore,
                            UnsubscribeBefore = x.Requirement.UnsubscribeBefore
                        }
                    }).ToList(),
                PositionManager = new PositionManager
                {
                    LimitType = (PositionType)((int)reception.PositionManager.LimitType),
                    Positions = reception.PositionManager.Positions.Select(p =>
                      new Position
                      {
                          Key = p.Key,
                          Time = p.Time,
                          IsActive = p.IsActive,
                          Histories = p.Histories.Select(h => 
                              new History
                              { 
                                  Action = h.Action, 
                                  DateTime = h.DateTime, 
                                  Object = h.Object, 
                                  Subject = h.Subject 
                              }).ToList(),
                          Payload = new PositionPayload
                          {
                              DisciplineKey = p.Payload.DisciplineKey,
                              ProgramKey = p.Payload.ProgramKey,
                              StudentKey = p.Payload.StudentKey,
                              Result = new PositionPayloadResult
                              {
                                  Comment = p.Payload.Result.Comment,
                                  TeacherKey = p.Payload.Result.TeacherKey,
                                  Score = new Score
                                  {
                                      Type = (ScoreType)((int)p.Payload.Result.Score.Type),
                                      Value = new Tuple<Type, object>(
                                          Type.GetType(p.Payload.Result.Score.Value.Item1), 
                                          p.Payload.Result.Score.Value.Item2
                                          )
                                  }
                              }
                          }
                      }).ToList()
                }
            };

            return item;
        }
    }
}
