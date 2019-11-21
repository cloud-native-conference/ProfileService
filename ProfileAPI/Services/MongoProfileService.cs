namespace ProfileAPI.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using ProfileAPI.Configuration;
    using ProfileAPI.Models;

    public class MongoProfileService
    {
        private readonly IMongoCollection<ProfileMongo> profiles;

        public MongoProfileService(IProfileDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            profiles = database.GetCollection<ProfileMongo>(settings.ProfileCollectionName);
        }

        public List<ProfileMongo> Get() =>
            profiles.Find(profile => true).ToList();

        public ProfileMongo Get(string id) =>
            profiles.Find<ProfileMongo>(profile => profile.UserPrincipalName == id).FirstOrDefault();

        public ProfileMongo Create(ProfileMongo profile)
        {
            profiles.InsertOne(profile);
            return profile;
        }

        public void Update(string id, ProfileMongo profileIn) =>
            profiles.ReplaceOne(profile => profile.UserPrincipalName == id, profileIn);
    }
}
