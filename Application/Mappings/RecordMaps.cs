using Mapster;

namespace Application.Mappings
{
    public class RecordMaps
    {
        public RecordMaps()
        {
            TypeAdapterConfig<Service.MongoDB.Model.Record, Domain.Record>
            .NewConfig()
                .Map(dest => dest.StudentKey, src => src.StudentKey)
                .Map(dest => dest.ProgramKey, src => src.ProgramKey)
                .Map(dest => dest.DisciplineKey, src => src.DisciplineKey)
                .Map(dest => dest.Result, src => src.Result.Adapt<Domain.Result>());

            TypeAdapterConfig<Domain.Record, Service.MongoDB.Model.Record>
            .NewConfig()
                .Map(dest => dest.StudentKey, src => src.StudentKey)
                .Map(dest => dest.ProgramKey, src => src.ProgramKey)
                .Map(dest => dest.DisciplineKey, src => src.DisciplineKey)
                .Map(dest => dest.Result, src => src.Result.Adapt<Service.MongoDB.Model.Result>());
        }
    }
}
