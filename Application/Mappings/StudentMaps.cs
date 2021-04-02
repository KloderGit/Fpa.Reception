using Domain;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using domain = Domain.Education;
using lcservice = Service.lC.Model;

namespace Application.Mappings
{
    public class StudentMaps
    {
        public StudentMaps()
        {
            TypeAdapterConfig<lcservice.Student, domain.Student>
            .NewConfig()
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Owner, src => src.Owner)
                .Map(dest => dest.Educations, src => src.Contract.Adapt<IEnumerable<domain.Student.StudentEducation>>());
        }
    }
}
