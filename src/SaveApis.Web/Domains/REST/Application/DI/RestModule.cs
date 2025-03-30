using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Web.Domains.Core.Infrastructure.DI;

namespace SaveApis.Web.Domains.REST.Application.DI;

public class RestModule(IAssemblyHelper helper) : BaseWebModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        var mvcBuilder = collection.AddControllers().AddNewtonsoftJson();
        foreach (var assembly in helper.GetRegisteredAssemblies())
        {
            mvcBuilder.AddApplicationPart(assembly);
        }

        builder.Populate(collection);
    }

    protected override void PostAuthentication(WebApplication application)
    {
        application.MapControllers();
    }
}
