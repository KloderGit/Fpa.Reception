using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Service.MongoDB;
using Service.MongoDB.Model;
using System;
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

            var mongo = new MongoRepository<ReceptionDto1>(settings);


            var item = new ReceptionDto1
            {
                Active = true,
                Enum = Tps.three,
                Tuple = new Tuple<int, int>(10,30),
                MyProperty = typeof(Score)
            };


            await mongo.InsertOneAsync(item);

            var sfdsdf = mongo.FilterBy(x => x.Active == true);

            var people = await mongo.FindOneAsync(x => x.Active == true);

            var ettt = people.ToJson();

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

