using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveApis.Web.Infrastructure.Builder;

namespace Example.Domains.Jwt.Application.Backend;

[ApiController]
[Route("api/jwt")]
public class JwtController(IJwtBuilder builder) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        var token = builder.Build();

        return Ok(token);
    }

    [Authorize]
    [HttpGet("authorized")]
    public IActionResult Authorized()
    {
        return Ok("Authorized");
    }
}
