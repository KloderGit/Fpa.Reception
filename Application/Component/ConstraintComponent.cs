using Domain;
using Domain.Interface;
using Mapster;
using Service.lC;

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


        public void Store(Constraint constraint)
        {
            var dto = constraint.Adapt<Service.MongoDB.Model.Constraint>();

            database.Constraints.InsertOne(dto);
        }

        public void CheckContract(Reception reception )
        {
            var value = reception.Date;
        }
    }
}
