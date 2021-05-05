using lc.fitnesspro.library.Interface;
using Microsoft.Extensions.Configuration;
using Service.lC.Manager;
using Service.lC.Provider;
using Service.lC.Repository;

namespace Service.lC
{
    public class Context
    {
        private readonly ProviderDepository providers;

        private ProgramManager program;
        private GroupManager group;
        private PersonManager person;
        private StudentManager student;
        private ContractManager contract;
        private EducationManager education;
        private ControlTypeManager controlType;

        public Context(
            BaseHttpClient httpClient,
            IManager lcManager,
            IConfiguration configuration)
        {
            var client = httpClient ?? throw new System.ArgumentNullException(nameof(httpClient));

            var repositories = new RepositoryDepository(client, configuration);
            providers = new ProviderDepository(repositories, lcManager);
        }

        public ProgramManager Program => program ??= new ProgramManager(
            providers.Program, 
            providers.Discipline, 
            providers.ControlType, 
            providers.EducationForm, 
            providers.Employee, 
            providers.Group);

        public GroupManager Group => @group ??= new GroupManager(providers.SubGroup);

        public ControlTypeManager ControlType => controlType ??= new ControlTypeManager(providers.ControlType, providers.ScoreType);

        public PersonManager Person => person ??= new PersonManager(
            providers.Person, 
            providers.Student);

        public StudentManager Student => student ??= new StudentManager(providers.Contract, providers.Student);

        public ContractManager Contract => contract ??= new ContractManager(
            providers.Contract, 
            providers.Program, 
            providers.Group, 
            providers.SubGroup);

        public EducationManager Education => education ??= new EducationManager(providers.Discipline, providers.Employee);
    }


}
