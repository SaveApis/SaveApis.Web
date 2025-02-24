using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SaveApis.Web.Infrastructure.Builder;

namespace SaveApis.Web.Application.Builder;

public class JwtBuilder(IConfiguration configuration) : IJwtBuilder
{
    private ICollection<Claim> Claims { get; } = [];

    public IJwtBuilder AddClaim(string type, string value)
    {
        return AddClaim(new Claim(type, value));
    }

    public IJwtBuilder AddClaim(Claim claim)
    {
        Claims.Add(claim);

        return this;
    }

    public IJwtBuilder AddClaims(params Claim[] claims)
    {
        foreach (var claim in claims)
        {
            AddClaim(claim);
        }

        return this;
    }

    public string Build()
    {
        var issuer = configuration["jwt_issuer"] ?? string.Empty;
        var audience = configuration["jwt_audience"] ?? string.Empty;
        var key = configuration["jwt_key"] ?? string.Empty;
        var expiration = configuration["jwt_expiration"] ?? string.Empty;

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.AddHours(double.TryParse(expiration, out var e) ? e : 8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature),
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateJwtSecurityToken(descriptor);

        return handler.WriteToken(token);
    }

    public Task<string> BuildAsync()
    {
        return Task.FromResult(Build());
    }
}
