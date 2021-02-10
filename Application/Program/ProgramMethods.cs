using Application.HttpClient;
using Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Service.MongoDB;
using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Program
{
    public class ProgramMethods
    {
        private readonly ProgramHttpClient http;


        public ProgramMethods(ProgramHttpClient client)
        {
            http = client;
        }

        public async Task<IEnumerable<ProgramDto>> GetByDiscipline(IEnumerable<Guid> keys)
        {
            return await http.FindByDiscipline(keys);
        }

        public async Task<IEnumerable<ProgramDto>> Find(IEnumerable<Guid> keys)
        {
            return await http.Find(keys);
        }

        public async Task InsertMongo()
        {



            var settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://localhost:7010",
                DatabaseName = "Reception"
            };

            var mongo = new MongoRepository<ReceptionDto>(settings);


            var item = new Reception
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
                    LimitType = PositionType.Seating,
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
                                  Score = new Score{
                                     Type = ScoreType.Hundred,
                                     Value = new Tuple<Type, object>(typeof(int), 100)
                                  }
                              }
                        }
                   }
                  },

                }
            };


            var dto = item.ConvertToType<ReceptionDto>(Convert.ConvertToMongoDto);


            await mongo.InsertOneAsync(dto);

            var people = mongo.FilterBy(x => x.IsActive == true);

            var ettt = item.ConvertFromType( Convert.ConvertFromMongoDto, people.First());

        }
    }

    [BsonIgnoreExtraElements]
    [BsonCollection("Receptions")]
    public class ReceptionDto1 : Reception, IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; }
    }
}
