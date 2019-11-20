using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProfileAPI.Models;
using ProfileAPI.Services;

namespace ProfileAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfileController: ControllerBase
    {
        private readonly ProfileService _profileService;
        private readonly PeopleGraphService _peopleGraphService;
        
        public ProfileController(ProfileService profileService, PeopleGraphService peopleGraphService)
        {
            _profileService = profileService;
            _peopleGraphService = peopleGraphService;
        }

        [HttpGet]
        public ActionResult<List<Profile>> Get()
        {          
            var mongoProfiles = _profileService.Get();
            var resultProfiles = new List<Profile>(mongoProfiles.Count);

            foreach (var profile in mongoProfiles)
            {
                var profileGraph = _peopleGraphService.Get(profile.UserPrincipalName);
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
            var profileMongo = _profileService.Get(upn);
            var profileGraph = _peopleGraphService.Get(upn);

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
            _profileService.Create(profile);

            return CreatedAtRoute("GetProfile", new { id = profile.Id.ToString() }, profile);
        }

        [HttpPut("{upn}")]
        public IActionResult Update(string upn, Profile profileIn)
        {
            var profile = _profileService.Get(upn);

            if (profile == null)
            {
                return NotFound();
            }

            _profileService.Update(upn, profileIn);

            return NoContent();
        }

        [HttpDelete("{upn}")]
        public IActionResult Delete(string upn)
        {
            var profile = _profileService.Get(upn);

            if (profile == null)
            {
                return NotFound();
            }

            _profileService.Remove(profile.UserPrincipalName);

            return NoContent();
        }

        
    }
}

