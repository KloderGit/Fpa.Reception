using Service.MongoDB;
using Service.MongoDB.Model;

namespace Service.MongoDB
{
    public class MongoContext
    {
        private readonly IMongoDbSettings options;

        private ReceptionProvider receptions;
        private IMongoRepository<BaseConstraintDto> constraints;
        private Settings settings;

        public MongoContext(IMongoDbSettings options) => this.options = options;

        public ReceptionProvider Receptions => receptions ??= new ReceptionProvider(new MongoRepository<Reception>(options));
        public IMongoRepository<BaseConstraintDto> Constraints => constraints ??= new MongoRepository<BaseConstraintDto>(options);

        public Settings Settings => settings ??= new Settings(options);
    }

    public class Settings
    {
        private readonly IMongoDbSettings options;

        public Settings(IMongoDbSettings options)
        {
            this.options = options;
        }

        IMongoRepository<TeacherSetting> teacher;
        IMongoRepository<GroupSettings> group;

        public IMongoRepository<TeacherSetting> Teacher => teacher ??= new MongoRepository<TeacherSetting>(options);
        public IMongoRepository<GroupSettings> Group => group ??= new MongoRepository<GroupSettings>(options);

    }
}
