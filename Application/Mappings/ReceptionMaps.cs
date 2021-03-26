using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public class ReceptionMaps
    {
        public ReceptionMaps()
        {
            TypeAdapterConfig<Service.MongoDB.Model.Reception, Domain.Reception>
            .NewConfig()
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.Date, src => src.Date)
                .Map(dest => dest.PositionManager, src => src.PositionManager.Adapt<Service.MongoDB.Model.PositionManager>())
                .Map(dest => dest.Events, src => src.Events.Adapt<List<Domain.Event>>())
                .Map(dest => dest.Histories, src => src.Histories);
        }
    }
}
