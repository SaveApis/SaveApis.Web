using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using SaveApis.Common.Domains.Core.Application.Helper;
using SaveApis.Common.Domains.Core.Infrastructure.Extensions;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Web.Domains.Correlate.Application.DI;
using SaveApis.Web.Domains.HangfireDashboard.Application.DI;
using SaveApis.Web.Domains.Jwt.Application.DI;
using SaveApis.Web.Domains.REST.Application.DI;
using SaveApis.Web.Domains.Swagger.Application.DI;

namespace SaveApis.Web.Domains.Core.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    private static AssemblyHelper AssemblyHelper { get; } = new AssemblyHelper(Assembly.GetExecutingAssembly());

    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        AssemblyHelper.RegisterAssemblies(assemblies);

        return builder;
    }

    public static WebApplicationBuilder WithSaveApis(this WebApplicationBuilder builder, HangfireType hangfireType, bool isBackend, Action<ContainerBuilder>? additionalModules = null)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
        {
            if (isBackend)
            {
                containerBuilder.WithWebModule<JwtModule>(builder.Configuration);
                containerBuilder.WithWebModule<RestModule>(AssemblyHelper);
                containerBuilder.WithWebModule<SwaggerModule>();
                containerBuilder.WithWebModule<CorrelateWebModule>();
                containerBuilder.WithWebModule<HangfireDashboardModule>(AssemblyHelper);
            }

            containerBuilder.WithCommonModules(AssemblyHelper, builder.Configuration, hangfireType, additionalModules);
        });

        return builder;
    }
}
