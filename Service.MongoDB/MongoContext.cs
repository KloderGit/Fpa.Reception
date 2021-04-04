using Service.MongoDB;
using Service.MongoDB.Model;

namespace Service.MongoDB
{
    public class MongoContext
    {
        private readonly IMongoDbSettings settings;

        private ReceptionProvider receptions;
        private IMongoRepository<Constraint> constraints;

        public MongoContext(IMongoDbSettings settings) => this.settings = settings;

        public ReceptionProvider Receptions => receptions ??= new ReceptionProvider(new MongoRepository<Reception>(settings));
        public IMongoRepository<Constraint> Constraints => constraints ??= new MongoRepository<Constraint>(settings);
    }
}
