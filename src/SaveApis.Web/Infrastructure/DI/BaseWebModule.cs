using Microsoft.AspNetCore.Builder;
using SaveApis.Common.Infrastructure.DI;

namespace SaveApis.Web.Infrastructure.DI;

public abstract class BaseWebModule : BaseModule
{
    protected virtual void PreAuthentication(WebApplication application)
    {
    }

    public virtual Task PreAuthenticationAsync(WebApplication application)
    {
        PreAuthentication(application);

        return Task.CompletedTask;
    }

    protected virtual void PostAuthentication(WebApplication application)
    {
    }

    public virtual Task PostAuthenticationAsync(WebApplication application)
    {
        PostAuthentication(application);

        return Task.CompletedTask;
    }
}
