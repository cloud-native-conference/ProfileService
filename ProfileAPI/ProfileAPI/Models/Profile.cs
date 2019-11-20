using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfileAPI.Models
{
    public class Profile
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string userPrincipalName { get; set; }

        public string displayName { get; set; }

        public string givenName { get; set; }

        public string surname { get; set; }

        public string mail { get; set; }

        public string jobTitle { get; set; }

        public string officeLocation { get; set; }

        public string Description { get; set; }

    }
}
