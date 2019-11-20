using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfileAPI.Models
{
    public class Profile
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        public string displayName { get; set; }

        public string Description { get; set; }
    }
}
