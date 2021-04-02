using Mapster;

namespace Application.Mappings
{
    public class ResultMaps
    {
        public ResultMaps()
        {
            TypeAdapterConfig<Service.MongoDB.Model.Result, Domain.Result>
            .NewConfig()
                .Map(dest => dest.TeacherKey, src => src.TeacherKey)
                //.Map(dest => dest.Score, src => src.Score.Adapt<Domain.Score>())
                .Map(dest => dest.Comment, src => src.Comment);

            TypeAdapterConfig<Domain.Result, Service.MongoDB.Model.Result>
            .NewConfig()
                .Map(dest => dest.TeacherKey, src => src.TeacherKey)
                //.Map(dest => dest.Score, src => src.Score.Adapt<Service.MongoDB.Model.Score>())
                .Map(dest => dest.Comment, src => src.Comment);
        }
    }
}
