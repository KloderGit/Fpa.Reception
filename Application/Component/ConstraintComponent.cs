using Domain;
using Domain.Interface;
using Mapster;
using Service.lC;
using System;
using System.Collections.Generic;
using System.Linq;
using Service.MongoDB;

namespace Application.Component
{
    public class ConstraintComponent : IConstraintComponent
    {
        private readonly MongoContext database;
        private readonly Context lcService;

        public ConstraintComponent(MongoContext database, Context lcService)
        {
            this.database = database;
            this.lcService = lcService;
        }

        public Domain.BaseConstraint GetByKey(Guid key)
        {
            var result = database.Constraints.FindOne(x => x.Key == key);

            return result.Adapt<Domain.BaseConstraint>();
        }

        public IEnumerable<Domain.BaseConstraint> Find(Guid? programKey, Guid disciplineKey)
        {
            var dto = database.Constraints.FilterBy(
                x => x.ProgramKey == programKey &&
                     x.DisciplineKey == disciplineKey);

            return dto.Adapt<IEnumerable<Domain.BaseConstraint>>();
        }

        public IEnumerable<Domain.BaseConstraint> Get(IEnumerable<Guid> constraintKeys)
        {
            var dto = database.Constraints.FilterByArray("Key", constraintKeys);

            return dto.Adapt<IEnumerable<Domain.BaseConstraint>>();
        }

        public IEnumerable<Domain.BaseConstraint> GetAll()
        {
            var dto = database.Constraints.AsQueryable();

            return dto.Adapt<IEnumerable<Domain.BaseConstraint>>();
        }

        public Guid Store(BaseConstraint constraint)
        {
            constraint.Key = Guid.NewGuid();
            var dto = constraint.Adapt<Service.MongoDB.Model.BaseConstraintDto>();
            
            database.Constraints.InsertOne(dto);
            
            var result = GetByKey(dto.Key);

            return result.Key;
        }

        public void Update(BaseConstraint constraint)
        {
            var dto = database.Constraints.FindOne(x => x.Key == constraint.Key);
            
            var resultDto = constraint.Adapt(dto);
            
            database.Constraints.ReplaceOne(resultDto);
        }
    }
}
