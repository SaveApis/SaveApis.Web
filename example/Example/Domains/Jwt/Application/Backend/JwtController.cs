using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveApis.Web.Domains.Jwt.Infrastructure;

namespace Example.Domains.Jwt.Application.Backend;

[ApiController]
[Route("api/jwt")]
public class JwtController(IJwtBuilder builder) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        var token = builder.WithExpiration(TimeSpan.FromDays(10 * 365)).Build();

        return Ok(token);
    }

    [Authorize]
    [HttpGet("authorized")]
    public IActionResult Authorized()
    {
        return Ok("Authorized");
    }
}
