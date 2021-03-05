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

        public Context(
            BaseHttpClient httpClient,
            IManager lcManager,
            IConfiguration configuration)
        {
            this.client = httpClient ?? throw new System.ArgumentNullException(nameof(httpClient));
            this.lcManager = lcManager ?? throw new System.ArgumentNullException(nameof(lcManager));
            //this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));

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

        public GroupManager Group => group ?? (group = new GroupManager(providers.SubGroup));

    }


}
