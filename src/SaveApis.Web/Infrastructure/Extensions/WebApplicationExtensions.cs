using Microsoft.AspNetCore.Builder;

namespace SaveApis.Web.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RunSaveApisAsync(this WebApplication application)
    {
        foreach (var actions in ContainerBuilderExtensions.PreAuthenticationActions)
        {
            await actions(application).ConfigureAwait(false);
        }

        application.UseAuthentication();
        application.UseAuthorization();

        foreach (var actions in ContainerBuilderExtensions.PostAuthenticationActions)
        {
            await actions(application).ConfigureAwait(false);
        }

        await application.RunAsync().ConfigureAwait(false);
    }
}
