using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Web.Domains.Core.Infrastructure.DI;

namespace SaveApis.Web.Domains.Swagger.Application.DI;

public class SwaggerModule : BaseWebModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddEndpointsApiExplorer();
        collection.AddSwaggerGen();

        builder.Populate(collection);
    }

    protected override void PostAuthentication(WebApplication application)
    {
        if (application.Environment.IsProduction())
        {
            return;
        }

        application.UseSwagger();
        application.UseSwaggerUI();
    }
}
