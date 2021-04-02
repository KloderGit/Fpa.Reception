using Mapster;
using System.Collections.Generic;

namespace Application.Mappings
{
    public class EventMaps
    {
        public EventMaps()
        {
            TypeAdapterConfig<Service.MongoDB.Model.Event, Domain.Event>
            .NewConfig()
                .Map(dest => dest.Discipline, src => src.Discipline.Adapt<Domain.BaseInfo>())
                .Map(dest => dest.Teachers, src => src.Teachers.Adapt<List<Domain.BaseInfo>>())
                .Map(dest => dest.Restrictions, src => src.Restrictions.Adapt<List<Domain.PayloadRestriction>>())
                .Map(dest => dest.Requirement, src => src.Requirement.Adapt<Domain.PayloadRequirement>());

            TypeAdapterConfig<Service.MongoDB.Model.Event, Domain.Event>
            .NewConfig()
                .Map(dest => dest.Discipline, src => src.Discipline.Adapt<Service.MongoDB.Model.BaseInfo>())
                .Map(dest => dest.Teachers, src => src.Teachers.Adapt<List<Service.MongoDB.Model.BaseInfo>>())
                .Map(dest => dest.Restrictions, src => src.Restrictions.Adapt<List<Service.MongoDB.Model.Restriction>>())
                .Map(dest => dest.Requirement, src => src.Requirement.Adapt<Service.MongoDB.Model.Requirement>());

            TypeAdapterConfig<Service.MongoDB.Model.Restriction, Domain.PayloadRestriction>
            .NewConfig()
                .Map(dest => dest.Program, src => src.Program)
                .Map(dest => dest.Group, src => src.Group)
                .Map(dest => dest.SubGroup, src => src.SubGroup)
                .Map(dest => dest.Option, src => src.Option.Adapt<Domain.PayloadOption>());

            TypeAdapterConfig<Domain.PayloadRestriction, Service.MongoDB.Model.Restriction>
            .NewConfig()
                .Map(dest => dest.Program, src => src.Program)
                .Map(dest => dest.Group, src => src.Group)
                .Map(dest => dest.SubGroup, src => src.SubGroup)
                .Map(dest => dest.Option, src => src.Option.Adapt<Service.MongoDB.Model.Option>());

            TypeAdapterConfig<Domain.PayloadOption, Service.MongoDB.Model.Option>
            .NewConfig()
                .Map(dest => dest.CheckAttemps, src => src.CheckAttemps)
                .Map(dest => dest.CheckDependings, src => src.CheckDependings)
                .Map(dest => dest.CheckContractExpired, src => src.CheckContractExpired)
                .Map(dest => dest.CheckAllowingPeriod, src => src.CheckAllowingPeriod);

            TypeAdapterConfig<Service.MongoDB.Model.Option, Domain.PayloadOption>
            .NewConfig()
                .Map(dest => dest.CheckAttemps, src => src.CheckAttemps)
                .Map(dest => dest.CheckDependings, src => src.CheckDependings)
                .Map(dest => dest.CheckContractExpired, src => src.CheckContractExpired)
                .Map(dest => dest.CheckAllowingPeriod, src => src.CheckAllowingPeriod);
        }
    }
}
