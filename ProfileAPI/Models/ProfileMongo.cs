namespace ProfileAPI.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;

    public class ProfileMongo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        public string Description { get; set; }
    }
}
