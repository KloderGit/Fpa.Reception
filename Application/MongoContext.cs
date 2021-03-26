using Service.MongoDB;
using Service.MongoDB.Model;

namespace Application
{
    public class MongoContext
    {
        private readonly IMongoDbSettings settings;

        private IMongoRepository<Reception> receptions;
        private IMongoRepository<Constraint> constraints;

        public MongoContext(IMongoDbSettings settings)
        {
            this.settings = settings;
        }

        public IMongoRepository<Reception> Receptions => receptions ?? (receptions = new MongoRepository<Reception>(settings));
        public IMongoRepository<Constraint> Constraints => constraints ?? (constraints = new MongoRepository<Constraint>(settings));
    }
}
