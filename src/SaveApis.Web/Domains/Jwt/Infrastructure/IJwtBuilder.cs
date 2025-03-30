using System.Security.Claims;
using SaveApis.Common.Domains.Builder.Infrastructure;

namespace SaveApis.Web.Domains.Jwt.Infrastructure;

public interface IJwtBuilder : IBuilder<string>
{
    IJwtBuilder AddClaim(string type, string value);
    IJwtBuilder AddClaim(Claim claim);
    IJwtBuilder AddClaims(params Claim[] claims);

    IJwtBuilder WithExpiration(TimeSpan expiration);
}
