using Calabonga.AspNetCore.AppDefinitions;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Implementations;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

namespace SewingFactory.Backend.WarehouseManagement.Web.Definitions.Provider;

public class ProvidersDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ILeftoverReportProvider, LeftoverReportProvider>();
        builder.Services.AddTransient<IAllOperationForStockReportProvider, AllOperationForStockReportProvider>();
        builder.Services.AddTransient<IAllSalesForGarmentModelReportProvider, AllSalesForGarmentModelReportProvider>();
        builder.Services.AddTransient<IAllOperationForPointOfSaleReportProvider, AllOperationForPointOfSaleReportProvider>();
    }
}
