namespace ProfileAPI.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using ProfileAPI.Configuration;
    using ProfileAPI.Models;

    public class ProfileService
    {
        private readonly IMongoCollection<Profile> profiles;

        public ProfileService(IProfileDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            profiles = database.GetCollection<Profile>(settings.ProfileCollectionName);
        }

        public List<Profile> Get() =>
            profiles.Find(profile => true).ToList();

        public Profile Get(string upn) =>
            profiles.Find<Profile>(profile => profile.UserPrincipalName == upn).FirstOrDefault();

        public Profile Create(Profile profile)
        {
            profiles.InsertOne(profile);
            return profile;
        }

        public void Update(string upn, Profile profileIn) =>
            profiles.ReplaceOne(profile => profile.UserPrincipalName == upn, profileIn);

        public void Remove(Profile profileIn) =>
            profiles.DeleteOne(profile => profile.UserPrincipalName == profileIn.UserPrincipalName);

        public void Remove(string upn) =>
            profiles.DeleteOne(profile => profile.UserPrincipalName == upn);
    }
}
