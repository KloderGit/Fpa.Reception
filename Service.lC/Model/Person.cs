using Service.lC.Interface;
using System;
using System.Collections.Generic;

namespace Service.lC.Model
{
    public class Person : Base, IConvert<Person>
    {
        public string Skype { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<Student> Students { get; set; }

        public TResult ConvertTo<TResult>(Func<Person, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public class Contact
        {
            public Guid Owner { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
}
