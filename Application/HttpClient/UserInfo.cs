using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Application.HttpClient
{
    public class UserInfo
    {
        [JsonProperty("sub")]
        public Guid Key { get; set; }
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }
        [JsonProperty("given_name")]
        public string Name { get; set; }
        [JsonProperty("middle_name")]
        public string SurName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("birthdate")]
        public DateTime Birthday { get; set; }
        [JsonProperty("name")]
        public string Login { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone_number")]
        public string Phone { get; set; }
    }
}
