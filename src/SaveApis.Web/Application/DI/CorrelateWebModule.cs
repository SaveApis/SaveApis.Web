using Autofac;
using Autofac.Extensions.DependencyInjection;
using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Web.Infrastructure.DI;

namespace SaveApis.Web.Application.DI;

public class CorrelateWebModule : BaseWebModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddCorrelate(options => options.IncludeInResponse = true);

        builder.Populate(collection);
    }

    protected override void PreAuthentication(WebApplication application)
    {
        application.UseCorrelate();
    }
}
