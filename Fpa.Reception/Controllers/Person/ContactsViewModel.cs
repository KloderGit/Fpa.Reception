using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reception.fitnesspro.ru.Misc;

namespace reception.fitnesspro.ru.Controllers.Person
{
    [PhoneOrEmailRequire]
    public class ContactsViewModel
    {
        [JsonProperty("phones")]
        public IEnumerable<string> Phones { get; set; }
        [JsonProperty("emails")]
        public IEnumerable<string> Emails { get; set; }
    }
}
