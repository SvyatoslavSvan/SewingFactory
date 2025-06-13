using Calabonga.AspNetCore.AppDefinitions;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Implementations;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;

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
