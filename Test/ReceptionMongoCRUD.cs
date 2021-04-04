using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Service.MongoDB;
using Service.MongoDB.Model;

namespace Test
{
    [TestClass]
    public class ReceptionMongoCrud
    {
        private IMongoRepository<Reception> mongo;

        public ReceptionMongoCrud()
        {
            var settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://localhost:7010",
                DatabaseName = "Reception"
            };

            var mongo = new MongoRepository<Reception>(settings);        
        }

        [TestMethod]
        public void CreateReception()
        {
            var settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://localhost:7010",
                DatabaseName = "Reception"
            };

            var mongo = new MongoRepository<Reception>(settings);   

            var reception = new Reception();

            reception.Date = DateTime.Now;
            reception.Key = new Guid("80f011db-4639-4157-bff5-ccb8da5e9025");

            var position = new PositionManager();
            position.Positions = new List<Position>();

            var pos1 = new Position();
            pos1.Key = new Guid("f9d94670-5fb3-11eb-8138-0cc47a4b75cc");
            pos1.Time = DateTime.Now;

            var pos2 = new Position();
            pos2.Key = new Guid("21d93649-a036-11e6-80e7-0cc47a4b75cc");
            pos2.Time = DateTime.Now;

            var arr = new List<Position>();

            arr.Add(pos1);
            arr.Add(pos2);

            position.Positions = arr;

            reception.PositionManager = position;

            mongo.InsertOne(reception);
        }

        [TestMethod]
        public void FindByPosition()
        {
            var settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://localhost:7010",
                DatabaseName = "Reception"
            };

            var mongo = new MongoRepository<Reception>(settings);

            var reception = mongo.FilterByPath("PositionManager.Positions.Key",
                new Guid("21d93649-a036-11e6-80e7-0cc47a4b75cc")).ToList();
        }

        [TestMethod]
        public void AddRecord()
        {
            var settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://localhost:7010",
                DatabaseName = "Reception"
            };

            var mongo = new MongoRepository<Reception>(settings);

            var reception = mongo.FilterByPath("PositionManager.Positions.Key",
                new Guid("21d93649-a036-11e6-80e7-0cc47a4b75cc")).ToList();

            var position = reception.SelectMany(x => x.PositionManager.Positions)
                .FirstOrDefault(x => x.Key == new Guid("21d93649-a036-11e6-80e7-0cc47a4b75cc"));


            var receord = new Record();
            receord.DisciplineKey = new Guid("b5a7a2a7-2ba7-4cf3-b647-2685618ec1c8");
            receord.ProgramKey = new Guid("4626d6eb-649f-480c-ba7a-fdefd87ed7c7");
            receord.StudentKey = new Guid("1f2fa592-b0a3-4b37-ab54-16fb71063aa7");

            position.Record = receord;

            mongo.ReplaceOne(reception.First());
        }

        public void SetRecord()
        {
            var client = new MongoClient("mongodb://localhost:7010");
            var database = client.GetDatabase("Reception");
            var collection = database.GetCollection<BsonDocument>("Receptions");

            var reception = mongo.FilterByPath("PositionManager.Positions.Key",
                new Guid("7046d9f9-b35f-eb11-8138-0cc47a4b75cc")).ToList();

            var position = reception.SelectMany(x => x.PositionManager.Positions)
                .FirstOrDefault(x => x.Key == new Guid("7046d9f9-b35f-eb11-8138-0cc47a4b75cc"));


            var receord = new Record();
            receord.DisciplineKey = new Guid("b5a7a2a7-2ba7-4cf3-b647-2685618ec1c8");
            receord.ProgramKey = new Guid("4626d6eb-649f-480c-ba7a-fdefd87ed7c7");
            receord.StudentKey = new Guid("1f2fa592-b0a3-4b37-ab54-16fb71063aa7");

            position.Record = receord;

            BsonValue vl = new BsonArray();

            var result = collection.UpdateOneAsync(
                new BsonDocument("PositionManager.Positions.Key", new Guid("7046d9f9-b35f-eb11-8138-0cc47a4b75cc").ToString()), 
                new BsonDocument("$set", new BsonDocument("Record", 28))).Result;

        }
    }
}
