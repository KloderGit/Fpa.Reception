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

        public async Task<Reception> GetByPosition(Guid positionKey)
        {
            var reception = await database.Receptions.GetByPosition(positionKey);

            var domen = reception.Adapt<IEnumerable<Domain.Reception>>();

            return domen.FirstOrDefault();
        }

        public async Task<Reception> Create(Reception reception)
        {
            var dto = reception.Adapt<Service.MongoDB.Model.Reception>();
                
            database.Receptions.Repository.InsertOne(dto);

            var serviceReception = await database.Receptions.GetByKeyAsync(reception.Key);

            var domen = reception.Adapt<Domain.Reception>();

            return domen;
        }

        public async Task Update(Reception reception)
        {
            var serviceReception = await database.Receptions.GetByKeyAsync(reception.Key);

            if(serviceReception == default) throw new ArgumentException("Рецепция не обнаружена");

            var dto = reception.Adapt(serviceReception);
                
            database.Receptions.Repository.ReplaceOne(dto);
        }

        public void Delete(Guid key)
        {
            database.Receptions.Repository.DeleteOne(x=>x.Key == key);
        }

        #region OLd

        [Obsolete]
        public async Task<IEnumerable<Reception>> GetByDisciplineKey(Guid discilineKey)
        {
            var dto = await database.Receptions.GetByDiscipline(discilineKey);

            var result = dto.Adapt<IEnumerable<Reception>>();

            return result;
        }

        public async Task<IEnumerable<Reception>> GetForPeriod(Guid? teacherKey, Guid? disciplineKey, DateTime startAfter, DateTime endBefore)
        {
            var dto = await database.Receptions.GetReceptionsForPeriod(teacherKey,disciplineKey,startAfter,endBefore);

            var result = dto.Adapt<IEnumerable<Reception>>();

            return result;
        }

        #endregion
    }
}
