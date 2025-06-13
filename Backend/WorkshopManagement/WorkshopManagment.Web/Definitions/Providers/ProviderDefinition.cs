using Calabonga.AspNetCore.AppDefinitions;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Providers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Providers;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.Providers;

public class ProviderDefinition : AppDefinition
{
    public override void ConfigureServices(
        WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        builder.Services.AddTransient<IReportProvider, ClosedXmlReportProvider>();
    }
}