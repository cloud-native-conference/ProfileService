﻿namespace ProfileAPI.Models
{
    using Newtonsoft.Json;

    public class ProfileGraph
    {
        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("mail")]
        public string Mail { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }
    }
}
