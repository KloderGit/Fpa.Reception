using Application.Component;
using Domain.Interface;
using lc.fitnesspro.library.Interface;
using Microsoft.Extensions.Configuration;
using Service.lC;
using Service.MongoDB;

namespace Application
{
    public class lcAppContext : IAppContext
    {
        Service.lC.Context lcContext;
        MongoContext mongo;

        IEducationComponent education;
        IPersonComponent person;
        IStudentComponent student;
        ITeacherComponent teacher;
        IReceptionComponent reception;
        IConstraintComponent constraint;
        IValidateComponent validator;

        public lcAppContext(
            BaseHttpClient httpClient,
            IManager lcManager,
            IMongoDbSettings dbSettings,
            IConfiguration configuration)
        {
            lcContext = new Context(httpClient, lcManager, configuration);
            mongo = new MongoContext(dbSettings);
        }

        public IEducationComponent Education => education ?? (education = new EducationComponent(lcContext));
        public IPersonComponent Person => person ?? (person = new PersonComponent(lcContext));
        public IStudentComponent Student => student ?? (student = new StudentComponent(mongo, lcContext));
        public ITeacherComponent Teacher => teacher ?? (teacher = new TeacherComponent(lcContext));
        public IReceptionComponent Reception => reception ?? (reception = new ReceptionComponent(mongo, lcContext));
        public IConstraintComponent Constraint => constraint ?? (constraint = new ConstraintComponent(mongo, lcContext));
        public IValidateComponent Validator => validator ?? (validator = new ValidateComponent(mongo, lcContext));
    }
}
