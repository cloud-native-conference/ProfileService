namespace ProfileAPI.Services
{
    using System.Threading.Tasks;
    using ProfileAPI.Models;
    
    public class ProfileService
    {
        private readonly MongoProfileService mongoProfileService;
        private readonly PeopleGraphService peopleGraphService;

        public ProfileService(MongoProfileService profileService, PeopleGraphService peopleGraphService)
        {
            this.mongoProfileService = profileService;
            this.peopleGraphService = peopleGraphService;
        }

        public async Task<Profile> GetProfileAsync(string token)
        {
            ProfileGraph profileGraph = await this.peopleGraphService.GetProfileAsync(token).ConfigureAwait(false);

            if (profileGraph == null)
            {
                return null;
            }

            ProfileMongo profileMongo = this.mongoProfileService.Get(profileGraph.UserPrincipalName);

            Profile mergedProfile = this.MergeProfile(profileGraph, profileMongo);
            return mergedProfile;
        }

        public async Task<Profile> GetProfileAsync(string token, string id)
        {
            ProfileGraph profileGraph = await this.peopleGraphService.GetProfileAsync(token, id).ConfigureAwait(false);

            if (profileGraph == null)
            {
                return null;
            }

            ProfileMongo profileMongo = this.mongoProfileService.Get(id);

            Profile mergedProfile = this.MergeProfile(profileGraph, profileMongo);
            return mergedProfile;
        }

        public async Task<bool> UpdateAsync(string token, string id, string description)
        {
            var profileGraph = await this.peopleGraphService.GetProfileAsync(token).ConfigureAwait(false);

            // Only allowed to update your own profile
            if (profileGraph.UserPrincipalName != id)
            {
                return false;
            }

            var profileMongo = this.mongoProfileService.Get(id);

            var profileIn = new ProfileMongo
            {
                UserPrincipalName = id,
                Description = description
            };

            // If the profile is not in the database but it is in the MicrosoftGraph, add 
            if (profileMongo == null)
            {
                mongoProfileService.Create(profileIn);
            }
            else
            {
                profileIn.Id = profileMongo.Id;
                mongoProfileService.Update(id, profileIn);
            }

            return true;
        }

        private Profile MergeProfile(ProfileGraph profileGraph, ProfileMongo profileMongo)
        {
            var profile = new Profile
            {
                DisplayName = profileGraph.DisplayName,
                GivenName = profileGraph.GivenName,
                Surname = profileGraph.Surname,
                Mail = profileGraph.Mail,
                JobTitle = profileGraph.JobTitle,
                OfficeLocation = profileGraph.OfficeLocation
            };

            if (profileMongo != null)
            {
                profile.Description = profileMongo.Description;
            }

            return profile;
        }
    }
}
