using Application.Component;
using Domain.Interface;
using lc.fitnesspro.library.Interface;
using Microsoft.Extensions.Configuration;
using Service.lC;
using Service.MongoDB;
using Service.Schedule.MySql;

namespace Application
{
    public class lcAppContext : IAppContext
    {
        private readonly IScheduleService schedule;
        Service.lC.Context lcContext;
        MongoContext mongo;

        IEducationComponent education;
        IPersonComponent person;
        IStudentComponent student;
        ITeacherComponent teacher;
        IReceptionComponent reception;
        ISettingComponent constraint;
        IValidateComponent validator;

        public lcAppContext(
            BaseHttpClient httpClient,
            IScheduleService schedule,
            IManager lcManager,
            IMongoDbSettings dbSettings,
            IConfiguration configuration)
        {
            lcContext = new Context(httpClient, lcManager, configuration);
            mongo = new MongoContext(dbSettings);
            this.schedule = schedule;
        }

        public IEducationComponent Education => education ?? (education = new EducationComponent(lcContext));
        public IPersonComponent Person => person ?? (person = new PersonComponent(lcContext));
        public IStudentComponent Student => student ?? (student = new StudentComponent(mongo, lcContext));
        public ITeacherComponent Teacher => teacher ?? (teacher = new TeacherComponent(mongo, lcContext));
        public IReceptionComponent Reception => reception ?? (reception = new ReceptionComponent(mongo, lcContext));
        public ISettingComponent Setting => constraint ?? (constraint = new SettingComponent(mongo, lcContext,schedule));
        public IValidateComponent Validator => validator ?? (validator = new ValidateComponent(mongo, lcContext));
    }
}
