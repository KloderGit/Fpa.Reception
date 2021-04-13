using Domain;
using Domain.Interface;
using Mapster;
using Service.lC;
using Service.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Reception GetByPosition(Guid positionKey)
        {
            throw new NotImplementedException();
        }

        public Reception Create(Reception reception)
        {
            throw new NotImplementedException();
        }

        public Reception Update(Reception reception)
        {
            throw new NotImplementedException();
        }

        public Reception Delete(Guid key)
        {
            throw new NotImplementedException();
        }

        #region OLd

        [Obsolete]
        public async Task<IEnumerable<Reception>> GetByDisciplineKey(Guid discilineKey)
        {
            var dto = await database.Receptions.GetByDiscipline(discilineKey);

            var result = dto.Adapt<IEnumerable<Reception>>();

            return result;
        }

        #endregion
    }
}
