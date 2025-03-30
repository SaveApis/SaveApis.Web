using Microsoft.Extensions.Configuration;

namespace SaveApis.Web.Domains.Core.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static bool IsBackend(this IConfiguration configuration)
    {
        return bool.TryParse(configuration["backend"], out var isBackend) && isBackend;
    }
}
