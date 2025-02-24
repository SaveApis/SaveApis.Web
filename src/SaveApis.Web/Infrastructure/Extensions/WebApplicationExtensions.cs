using Microsoft.AspNetCore.Builder;
using SaveApis.Common.Domain.Types;

namespace SaveApis.Web.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RunSaveApisAsync(this WebApplication application, ApplicationType applicationType)
    {
        foreach (var actions in ContainerBuilderExtensions.PreAuthenticationActions)
        {
            await actions(application).ConfigureAwait(false);
        }

        if (applicationType == ApplicationType.Backend)
        {
            application.UseAuthentication();
            application.UseAuthorization();
        }

        foreach (var actions in ContainerBuilderExtensions.PostAuthenticationActions)
        {
            await actions(application).ConfigureAwait(false);
        }

        await application.RunAsync().ConfigureAwait(false);
    }
}
