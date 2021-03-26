using Application.Extensions;
using Mapster;
using System.Linq;
using domain = Domain.Education;
using lcservice = Service.lC.Model;

namespace Application.Mappings
{
    internal class PersonMaps
    {
        public PersonMaps()
        {
            TypeAdapterConfig<lcservice.Person, domain.Person>
            .NewConfig()
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Contacts, src => new domain.Person.PersonContact
                {
                    Skype = src.Skype,
                    Phones = src.Contacts.Where(x => x.Phone.IsNotEmpty()).Select(p => p.Phone),
                    Emails = src.Contacts.Where(x => x.Email.IsNotEmpty()).Select(p => p.Email)
                });
        }
    }
}
