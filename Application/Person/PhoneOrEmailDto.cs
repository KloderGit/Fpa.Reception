using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Person
{
    public class PhoneOrEmailDto
    {
        [JsonProperty("phones")]
        public IEnumerable<string> Phones { get; set; }
        [JsonProperty("emails")]
        public IEnumerable<string> Emails { get; set; }
    }
}
