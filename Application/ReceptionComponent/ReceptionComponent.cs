using Application.ReceptionComponent.Converter;
using Service.MongoDB;
using Service.MongoDB.Model;

namespace Application.ReceptionComponent
{
    public class ReceptionComponent
    {
        private readonly IMongoRepository<ReceptionDto> database;

        public ReceptionComponent(IMongoRepository<ReceptionDto> database)
        {
            this.database = database;
        }

        public void StoreReception(Domain.Reception reception)
        {
            var dto = reception.ConvertToType<ReceptionDto>(ReceptionConverter.ConvertToMongoDto);

            database.InsertOne(dto);
        }
    }
}
