using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SaveApis.Web.Domains.Jwt.Infrastructure;

namespace SaveApis.Web.Domains.Jwt.Application.Builder;

public class JwtBuilder(IConfiguration configuration) : IJwtBuilder
{
    private TimeSpan Expiration { get; set; } = TimeSpan.FromHours(8);
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
    public IJwtBuilder WithExpiration(TimeSpan expiration)
    {
        Expiration = expiration;

        return this;
    }

    public string Build()
    {
        var issuer = configuration["jwt_issuer"] ?? string.Empty;
        var audience = configuration["jwt_audience"] ?? string.Empty;
        var key = configuration["jwt_key"] ?? string.Empty;

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.Add(Expiration),
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
