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

        public IEnumerable<Domain.Constraint> Get(IEnumerable<Guid> constraintKeys)
        {
            var dto = database.Constraints.FilterByArray("Key", constraintKeys);

            return dto.Adapt<IEnumerable<Domain.Constraint>>();
        }

        public IEnumerable<Domain.Constraint> GetAll()
        {
            var dto = database.Constraints.AsQueryable();

            return dto.Adapt<IEnumerable<Domain.Constraint>>();
        }

        public void Store(Constraint constraint)
        {
            var dto = constraint.Adapt<Service.MongoDB.Model.Constraint>();

            database.Constraints.InsertOne(dto);
        }
    }
}
