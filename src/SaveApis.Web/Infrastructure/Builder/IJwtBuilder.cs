using System.Security.Claims;
using SaveApis.Common.Infrastructure.Builder;

namespace SaveApis.Web.Infrastructure.Builder;

public interface IJwtBuilder : IBuilder<string>
{
    IJwtBuilder AddClaim(string type, string value);
    IJwtBuilder AddClaim(Claim claim);
    IJwtBuilder AddClaims(params Claim[] claims);
}
