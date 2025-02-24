using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Infrastructure.Extensions;
using SaveApis.Web.Application.DI;

namespace SaveApis.Web.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    private static AssemblyHelper Helper { get; } = new AssemblyHelper(Assembly.GetExecutingAssembly());

    public static WebApplicationBuilder WithAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        Helper.AddAssemblies(assemblies);

        return builder;
    }

    public static WebApplicationBuilder WithSaveApis(this WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
        {
            containerBuilder.WithWebModule<JwtModule>(builder.Configuration);

            containerBuilder.WithCommonModules(builder.Configuration, Helper);
        });

        return builder;
    }
}
