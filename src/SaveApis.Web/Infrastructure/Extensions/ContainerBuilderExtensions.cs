using Autofac;
using Microsoft.AspNetCore.Builder;
using SaveApis.Common.Infrastructure.Extensions;
using SaveApis.Web.Infrastructure.DI;

namespace SaveApis.Web.Infrastructure.Extensions;

public static class ContainerBuilderExtensions
{
    internal static ICollection<Func<WebApplication, Task>> PreAuthenticationActions { get; } = [];
    internal static ICollection<Func<WebApplication, Task>> PostAuthenticationActions { get; } = [];

    public static ContainerBuilder WithWebModule<TModule>(this ContainerBuilder builder, params object[] args) where TModule : BaseWebModule
    {
        builder.WithModule<TModule>(module =>
        {
            PreAuthenticationActions.Add(module.PreAuthenticationAsync);
            PostAuthenticationActions.Add(module.PostAuthenticationAsync);
        }, args);

        return builder;
    }
}
