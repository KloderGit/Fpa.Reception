using Application.ReceptionComponent.Converter;
using Mapster;
using Service.MongoDB;
using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.ReceptionComponent
{
    public class ReceptionComponent
    {
        private readonly IMongoRepository<Reception> database;

        public ReceptionComponent(IMongoRepository<Reception> database)
        {
            this.database = database;
        }

        public IEnumerable<Reception> GetReceptionByDisciplineKey(Guid key)
        {
            var dto = database.FilterByPath("Events.Discipline.Key", key);

            return dto.ToList();
        }

        public void StoreReception(Domain.Reception reception)
        {
            var dto = reception.ConvertToType<Reception>(ReceptionConverter.ConvertToMongoDto);

            database.InsertOne(dto);
        }

        public IEnumerable<Reception> GetAll()
        {
            var dto = database.AsQueryable();

            return dto.ToList();
        }

        public object GetReceptionByTeacherKey(Guid key)
        {
            var dto = database.FilterByPath("Events.Teachers.Key", key);

            return dto.ToList();
        }
    }
}
