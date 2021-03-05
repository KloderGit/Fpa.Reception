using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Repository;

namespace Service.lC.Provider
{
    public class GenericProvider<T, TDto> : IProvider<T, TDto> where T : Base
    {
        public IRepositoryAsync<T, TDto> Repository { get; private set; }

        public GenericProvider(
                IRepositoryAsync<T, TDto> repository
            )
        {
            this.Repository = repository;
        }
    }
}
