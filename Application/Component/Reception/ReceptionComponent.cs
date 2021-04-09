using Domain;
using Domain.Interface;
using Mapster;
using Service.lC;
using Service.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Component
{
    public class ReceptionComponent : IReceptionComponent
    {
        private readonly MongoContext database;

        public ReceptionComponent(MongoContext database, Context lcContext)
        {
            this.database = database;
        }

        public IEnumerable<Reception> Get()
        {
            var dto = database.Receptions.Repository.AsQueryable().ToList();

            var result = dto.Adapt<List<Reception>>();

            return result;
        }

        public Reception GetByKey(Guid key)
        {
            var dto = database.Receptions.Repository.FindOne(x => x.Key == key);

            var result = dto.Adapt<Reception>();

            return result;
        }

        public void CreateReception(Reception reception)
        {
            var dto = reception.Adapt<Service.MongoDB.Model.Reception>();
                
            database.Receptions.Repository.InsertOne(dto);
        }

        public void CreateRecord(Reception reception)
        {
            throw new NotImplementedException();
        }
    }
}
