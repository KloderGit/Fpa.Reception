using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Repository;

namespace Service.lC.Provider
{
    public class GenericProvider<T> : IProvider<T> where T : Base
    {
        private readonly RepositoryDepository depository;
        public IRepositoryAsync<T> Repository { get; private set; }

        public GenericProvider(
            IRepositoryAsync<T> repository,
            RepositoryDepository depository
            )
        {
            this.Repository = repository;
            this.depository = depository;
        }

    }
}
