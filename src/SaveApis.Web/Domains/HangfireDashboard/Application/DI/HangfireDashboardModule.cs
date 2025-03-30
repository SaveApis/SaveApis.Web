using Autofac;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Web.Domains.Core.Infrastructure.DI;
using Serilog;

namespace SaveApis.Web.Domains.HangfireDashboard.Application.DI;

public class HangfireDashboardModule(IAssemblyHelper helper) : BaseWebModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(helper.GetRegisteredAssemblies().ToArray())
            .Where(type => type.IsAssignableTo<IDashboardAuthorizationFilter>() || type.IsAssignableTo<IDashboardAsyncAuthorizationFilter>())
            .AsImplementedInterfaces();
    }

    protected override void PostAuthentication(WebApplication application)
    {
        var scope = application.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        var filters = scope.ServiceProvider.GetRequiredService<IEnumerable<IDashboardAuthorizationFilter>>().ToList();
        var asyncFilters = scope.ServiceProvider.GetRequiredService<IEnumerable<IDashboardAsyncAuthorizationFilter>>().ToList();

        if (!application.Environment.IsDevelopment() && filters.Count == 0 && asyncFilters.Count == 0)
        {
            logger.Warning("No Hangfire dashboard authorization filters found. Dashboard will be disabled.");

            return;
        }

        var title = application.Configuration["hangfire_title"] ?? "Hangfire Dashboard";
        var options = new DashboardOptions
        {
            Authorization = filters,
            AsyncAuthorization = asyncFilters,
            DashboardTitle = title,
            DarkModeEnabled = true,
            DisplayStorageConnectionString = application.Environment.IsDevelopment(),
        };

        application.MapHangfireDashboard(options);
    }
}
