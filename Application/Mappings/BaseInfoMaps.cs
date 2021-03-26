using Mapster;

namespace Application.Mappings
{
    public class BaseInfoMaps
    {
        public BaseInfoMaps()
        {
            TypeAdapterConfig<Service.MongoDB.Model.BaseInfo, Domain.BaseInfo>
            .NewConfig()
                               .Map(dest => dest.Key, src => src.Key)
                               .Map(dest => dest.Title, src => src.Title);
        }
    }
}
