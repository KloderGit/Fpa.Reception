using System.Collections.Generic;

namespace Domain.Education
{
    public class Person : BaseInfo
    {
        public PersonContact Contacts { get; set; }

        public List<Student> Students { get; set; }

        public class PersonContact
        {
            public string Skype { get; set; }
            public IEnumerable<string> Phones { get; set; }
            public IEnumerable<string> Emails { get; set; }
        }
    }
}
