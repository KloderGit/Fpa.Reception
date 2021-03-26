using Mapster;
using System.Collections.Generic;

namespace Application.Mappings
{
    public class PositionMaps
    {
        public PositionMaps()
        {
            TypeAdapterConfig<Service.MongoDB.Model.PositionManager, Domain.PositionManager>
            .NewConfig()
                .Map(dest => dest.LimitType, src => (Domain.PositionType)(int)src.LimitType)
                .Map(dest => dest.Positions, src => src.Positions.Adapt<List<Domain.Position>>());

            TypeAdapterConfig<Domain.PositionManager, Service.MongoDB.Model.PositionManager>
            .NewConfig()
                .Map(dest => dest.LimitType, src => (Service.MongoDB.Model.PositionTypeDto)(int)src.LimitType)
                .Map(dest => dest.Positions, src => src.Positions.Adapt<IEnumerable<Service.MongoDB.Model.Position>>());


            TypeAdapterConfig<Service.MongoDB.Model.Position, Domain.Position>
            .NewConfig()
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.Time, src => src.Time)
                .Map(dest => dest.Record, src => src.Record.Adapt<Domain.Record>())
                .Map(dest => dest.Histories, src => src.Histories);

            TypeAdapterConfig<Domain.Position, Service.MongoDB.Model.Position>
            .NewConfig()
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.Time, src => src.Time)
                .Map(dest => dest.Record, src => src.Record.Adapt<Service.MongoDB.Model.Record>())
                .Map(dest => dest.Histories, src => src.Histories);
        }
    }
}
