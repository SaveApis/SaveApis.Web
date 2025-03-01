using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Domain.Types;
using SaveApis.Common.Infrastructure.Extensions;
using SaveApis.Web.Application.DI;

namespace SaveApis.Web.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    private static AssemblyHelper AssemblyHelper { get; } = new(Assembly.GetExecutingAssembly());

    public static ApplicationType ReadApplicationType(this WebApplicationBuilder builder)
    {
        return Enum.TryParse(builder.Configuration["application_type"] ?? string.Empty, true, out ApplicationType type)
            ? type
            : ApplicationType.Server;
    }

    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        AssemblyHelper.AddAssemblies(assemblies);

        return builder;
    }

    public static WebApplicationBuilder WithSaveApis(this WebApplicationBuilder builder, ApplicationType applicationType)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
        {
            if (applicationType == ApplicationType.Backend)
            {
                containerBuilder.WithWebModule<JwtModule>(builder.Configuration);
                containerBuilder.WithWebModule<RestModule>(AssemblyHelper);
                containerBuilder.WithWebModule<CorrelateWebModule>();
                containerBuilder.WithWebModule<HangfireDashboardModule>(AssemblyHelper);
            }

            containerBuilder.WithCommonModules(builder.Configuration, AssemblyHelper, applicationType);
        });

        return builder;
    }
}
