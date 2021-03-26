using Mapster;
using domain = Domain.Education;
using lcservice = Service.lC.Model;

namespace Application.Mappings
{
    public class ContractMaps
    {
        public ContractMaps()
        {
            TypeAdapterConfig<lcservice.Contract, domain.Student.StudentContract>
            .NewConfig()
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.StartEducationDate, src => src.StartEducationDate)
                .Map(dest => dest.FinishEducationDate, src => src.FinishEducationhDate)
                .Map(dest => dest.ExpiredDate, src => src.ExpiredDate);

            TypeAdapterConfig<lcservice.Contract, domain.Student.StudentEducation>
            .NewConfig()
                .Map(dest => dest.EducationProgram, src => src.EducationProgram)
                .Map(dest => dest.Group, src => src.Group)
                .Map(dest => dest.SubGroup, src => src.SubGroup)
                .Map(dest => dest.Contract, src => src.Adapt<domain.Student.StudentContract>());
        }
    }
}
