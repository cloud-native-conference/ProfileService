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

        public ProfileController(ProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<Profile>> GetMe()
        {
            var token = Request.Headers["Authorization"];
            return await this.profileService.GetProfileAsync(token);
        }

        [HttpGet("users/{id}", Name = "GetProfile")]
        public async Task<ActionResult<Profile>> GetAsync(string id)
        {
            var token = Request.Headers["Authorization"];
            var profile = await this.profileService.GetProfileAsync(token, id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] string description)
        {
            var token = Request.Headers["Authorization"];
            var status = await profileService.UpdateAsync(token, id, description);

            if (status == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

