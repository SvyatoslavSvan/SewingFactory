using Calabonga.AspNetCore.AppDefinitions;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides;

namespace SewingFactory.Backend.WarehouseManagement.Web.Definitions.Provider;

public class ProvidersDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder) => builder.Services.AddTransient<ILeftoverReportProvider, LeftoverReportProvider>();
}
