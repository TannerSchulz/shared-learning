using FLP.Client.Services.UserAccount;
using FLP.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace FLP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAccountController : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromServices] IUserAccountService UserAccountService, RegisterModel model)
    {
        var result = await UserAccountService.Register(model);
        if(result.Code == "200")
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromServices] IUserAccountService UserAccountService, LoginModel model)
    {
        var result = await UserAccountService.Login(model);
        if(result.Code == "200")
        {
            return Ok(result);
        }
        if(result.Code == "401")
        {
            return Unauthorized(result);
        }
        return BadRequest(result);
    }
}
