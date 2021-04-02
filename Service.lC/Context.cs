using lc.fitnesspro.library.Interface;
using Microsoft.Extensions.Configuration;
using Service.lC.Manager;
using Service.lC.Provider;
using Service.lC.Repository;

namespace Service.lC
{
    public class Context
    {
        private readonly RepositoryDepository repositories;
        private readonly ProviderDepository providers;

        private readonly BaseHttpClient client;
        private readonly IManager lcManager;
        private readonly IConfiguration configuration;

        private ProgramManager program;
        private GroupManager group;
        private PersonManager person;
        private StudentManager student;
        private ContractManager contract;
        private EducationManager education;

        public Context(
            BaseHttpClient httpClient,
            IManager lcManager,
            IConfiguration configuration)
        {
            this.client = httpClient ?? throw new System.ArgumentNullException(nameof(httpClient));

            repositories = new RepositoryDepository(client, configuration);
            providers = new ProviderDepository(repositories, lcManager);
        }

        public ProgramManager Program => program ?? (
                program = new ProgramManager(
                    providers.Program, 
                    providers.Discipline, 
                    providers.ControlType, 
                    providers.EducationForm, 
                    providers.Employee, 
                    providers.Group));

        public GroupManager Group => group ?? (
            group = new GroupManager(providers.SubGroup));

        public PersonManager Person => person ?? (
            person = new PersonManager(
                providers.Person, 
                providers.Student));

        public StudentManager Student => student ?? (
            student = new StudentManager(providers.Contract, providers.Student));

        public ContractManager Contract => contract ?? (
            contract = new ContractManager(
                providers.Contract, 
                providers.Program, 
                providers.Group, 
                providers.SubGroup));

        public EducationManager Education => education ?? (
            education = new EducationManager( providers.Discipline) );
    }


}
