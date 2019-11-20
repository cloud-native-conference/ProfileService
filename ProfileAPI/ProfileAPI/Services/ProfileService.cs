using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using ProfileAPI.Configuration;
using ProfileAPI.Models;

namespace ProfileAPI.Services
{
    public class ProfileService
    {
        private readonly IMongoCollection<Profile> _profiles;

        public ProfileService(IProfileDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _profiles = database.GetCollection<Profile>(settings.ProfileCollectionName);
        }

        public List<Profile> Get() =>
            _profiles.Find(profile => true).ToList();

        public Profile Get(string upn) =>
            _profiles.Find<Profile>(profile => profile.UserPrincipalName == upn).FirstOrDefault();

        public Profile Create(Profile profile)
        {
            _profiles.InsertOne(profile);
            return profile;
        }

        public void Update(string upn, Profile profileIn) =>
            _profiles.ReplaceOne(profile => profile.UserPrincipalName == upn, profileIn);

        public void Remove(Profile profileIn) =>
            _profiles.DeleteOne(profile => profile.UserPrincipalName == profileIn.UserPrincipalName);

        public void Remove(string upn) =>
            _profiles.DeleteOne(profile => profile.UserPrincipalName == upn);
    }
}
