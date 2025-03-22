using FLP.Client.Services.Profile;
using FLP.Client.Services.UserAccount;
using FLP.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FLP.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateProfile([FromServices] IProfileService ProfileService, ProfileDto profileDto)
    {
        var result = await ProfileService.CreateProfile(profileDto);
        if (result.Code == "200")
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<ProfileDto>> GetUserProfile([FromServices] IProfileService ProfileService)
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if(string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized("No token provided");
        }
        var token = authHeader["Bearer ".Length..];

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var email = jwtToken.Claims.FirstOrDefault(o => o.Type == "email")?.Value;

        return Ok(await ProfileService.GetUserProfile(email));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveProfile([FromServices] IProfileService ProfileService, ProfileDto profileDto)
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized("No token provided");
        }
        var token = authHeader["Bearer ".Length..];

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var email = jwtToken.Claims.FirstOrDefault(o => o.Type == "email")?.Value;

        await ProfileService.SaveProfile(profileDto, email);

        return Ok();
    }
}
