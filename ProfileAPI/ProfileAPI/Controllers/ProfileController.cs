namespace ProfileAPI.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using ProfileAPI.Models;
    using ProfileAPI.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController: ControllerBase
    {
        private readonly ProfileService profileService;
        private readonly PeopleGraphService peopleGraphService;
        
        public ProfileController(ProfileService profileService, PeopleGraphService peopleGraphService)
        {
            this.profileService = profileService;
            this.peopleGraphService = peopleGraphService;
        }

        [HttpGet]
        public ActionResult<List<Profile>> Get()
        {          
            var mongoProfiles = profileService.Get();
            var resultProfiles = new List<Profile>(mongoProfiles.Count);

            foreach (var profile in mongoProfiles)
            {
                var profileGraph = peopleGraphService.Get(profile.UserPrincipalName);
                if (profileGraph != null)
                {
                    profileGraph.Description = profile.Description;
                    resultProfiles.Add(profileGraph);
                }
            }

            return resultProfiles;
        }
            
        [HttpGet("{upn}", Name = "GetProfile")]
        public ActionResult<Profile> Get(string upn)
        {
            var profileMongo = profileService.Get(upn);
            var profileGraph = peopleGraphService.Get(upn);

            if (profileMongo == null || profileGraph == null)
            {
                return NotFound();
            }

            var mergedProfile = profileGraph;
            mergedProfile.Description = profileMongo.Description;

            return mergedProfile;
        }

        [HttpPost]
        public ActionResult<Profile> Create(Profile profile)
        {
            profileService.Create(profile);

            return CreatedAtRoute("GetProfile", new { id = profile.Id.ToString() }, profile);
        }

        [HttpPut("{upn}")]
        public IActionResult Update(string upn, Profile profileIn)
        {
            var profile = profileService.Get(upn);

            if (profile == null)
            {
                return NotFound();
            }

            profileIn.Id = profile.Id;
            profileService.Update(upn, profileIn);

            return NoContent();
        }

        [HttpDelete("{upn}")]
        public IActionResult Delete(string upn)
        {
            var profile = profileService.Get(upn);

            if (profile == null)
            {
                return NotFound();
            }

            profileService.Remove(profile.UserPrincipalName);

            return NoContent();
        }

        
    }
}

