using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.lC.Dto
{
    public class PersonDto : BaseDto, IConvert<PersonDto>
    {
        public string Skype { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }

        static PersonDto()
        {
            Converter.Register<PersonDto, Person>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<PersonDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static Person Convert(PersonDto dto)
        {
            var person = new Person
            {
                Key = dto.Key,
                Title = dto.Title,
                Skype = dto.Skype,
                Contacts = dto.Contacts.Select(x=> new Person.Contact { Owner = x.ParentKey, Email = x.Email, Phone =x.Phone })
            };

            return person;
        }

        public class Contact
        {
            public Guid ParentKey { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
}