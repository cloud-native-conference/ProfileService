namespace ProfileAPI.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;

    public class Profile
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userPrincipalName")]
        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [BsonElement("displayName")]
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [BsonElement("givenName")]
        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [BsonElement("surname")]
        [JsonProperty("surname")]
        public string Surname { get; set; }

        [BsonElement("mail")]
        [JsonProperty("mail")]
        public string Mail { get; set; }

        [BsonElement("jobTitle")]
        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [BsonElement("officeLocation")]
        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }

        public string Description { get; set; }
    }
}
