using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Service.MongoDB;
using System;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var settings = new MongoDbSettings()
            {
                ConnectionString = "http://localhost:7010",
                DatabaseName = "Reception"
            };

            var mongo = new MongoRepository<ReceptionDto>(settings);


            var item = new ReceptionDto
            {
                Date = DateTime.Now,
                IsActive = true,
                Key = Guid.NewGuid(),
                Histories = new List<History> { new History { Object = Guid.NewGuid(), Action = " создал ", Subject = Guid.NewGuid(), DateTime = DateTime.UtcNow } },
                Events = new List<ReceptionPayload> {
                         new ReceptionPayload {
                          Teachers = new List<BaseInfo>{ new BaseInfo { Key = Guid.NewGuid(), Title = "Меркурьев" }, new BaseInfo { Key = Guid.NewGuid(), Title = "Калашников" } },
                           Discipline =  new BaseInfo { Key = Guid.NewGuid(), Title = "Anatomy" },
                             Constraints = new List<PayloadConstraints>{ new PayloadConstraints { Program = Guid.NewGuid(), Group = Guid.NewGuid(), SubGroup = Guid.NewGuid(),
                                 Options = new PayloadOptions{
                                     CheckAttemps = true,
                                      CheckContractExpired = true,
                                       CheckDependings = false
                                 }
                             } },
                              Requirement = new PayloadRequirement{
                               AllowedAttempCount = 10,
                                SubscribeBefore = DateTime.UtcNow,
                                 UnsubscribeBefore = DateTime.Now,
                                  DependsOnOtherDiscipline = new List<Guid>{
                                   Guid.NewGuid()
                                  }
                              }
                         }
                     },
                PositionManager = new PositionManager()
                {
                    Positions = new List<Position> {
                   new Position{
                     Key = Guid.NewGuid(),
                      IsActive = true,
                       Time = DateTime.Now,
                        Payload = new PositionPayload{
                             DisciplineKey = Guid.NewGuid(),
                              ProgramKey = Guid.NewGuid(),
                               StudentKey = Guid.NewGuid(),
                                 Result = new PositionPayloadResult{
                                     TeacherKey = Guid.NewGuid(),
                                      Comment = "Rate comment",
                                       Score = new Hundred(45)
                                 }
                        }
                   }
                  }
                }
            };


            mongo.InsertOneAsync(item).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        public class ReceptionDto : Reception, IDocument
        {
            ObjectId IDocument.Id { get; set; }
            DateTime IDocument.CreatedAt { get; }
        }
    }
}
