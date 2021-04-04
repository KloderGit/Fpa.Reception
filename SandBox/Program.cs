using Application.Component;
using Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Service.MongoDB;
using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandBox
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://localhost:7010",
                DatabaseName = "Reception"
            };

            var mongo = new MongoRepository<Service.MongoDB.Model.Reception>(settings);

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
                                //Score = new Hundred(45)
                            }
                        }
                   }
                  }
                }
            };



            var dto = item.ConvertToType(ReceptionConverter.ConvertToMongoDto);


            await mongo.InsertOneAsync(dto);

            Console.WriteLine("Hello World!");
        }

    }

    [BsonIgnoreExtraElements]
    [BsonCollection("Receptions")]
    public class ReceptionDto1 : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; }

        public bool Active { get; set; }

        public Tps Enum { get; set; }

        public Type MyProperty { get; set; }

        public Tuple<int, int> Tuple { get; set; }
    }

    public enum Tps
    {
        one,
        two,
        three
    }
}

