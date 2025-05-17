using Calabonga.AspNetCore.AppDefinitions;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Providers;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.Providers
{
    public class ProviderDefinition : AppDefinition
    {
        public override void ConfigureServices(
            WebApplicationBuilder builder)
            => builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
    }
}