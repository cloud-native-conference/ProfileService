namespace ProfileAPI.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ProfileAPI.Models;
    using ProfileAPI.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService profileService;
        private readonly PeopleGraphService peopleGraphService;

        public ProfileController(ProfileService profileService, PeopleGraphService peopleGraphService)
        {
            this.profileService = profileService;
            this.peopleGraphService = peopleGraphService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<Profile>> GetMe()
        {
            var token = Request.Headers["Authorization"];
            return await this.peopleGraphService.GetProfileAsync(token);
        }

        [HttpGet("users/{upn}", Name = "GetProfile")]
        public async Task<ActionResult<Profile>> GetAsync(string upn)
        {
            var token = Request.Headers["Authorization"];
            var profileGraph = await peopleGraphService.GetAsync(token, upn);

            return profileGraph;
        }


        [HttpPut("users/{upn}")]
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


    }
}

