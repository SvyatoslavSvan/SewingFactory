using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Mapping.Converters;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Mapping.Profiles;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Mapping;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Mapping.Profiles;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Implementations;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public static class TestHelpers
{
    public static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(optionsAction: o =>
            o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddUnitOfWork<ApplicationDbContext>();

        services.AddTransient<GarmentCategoryStubConverter>();
        services.AddTransient<IAllOperationForPointOfSaleReportProvider, AllOperationForPointOfSaleReportProvider>();
        services.AddTransient<IAllOperationForStockReportProvider,      AllOperationForStockReportProvider>();
        services.AddTransient<IAllSalesForGarmentModelReportProvider,   AllSalesForGarmentModelReportProvider>();
        services.AddAutoMapper(
            configAction: (sp, cfg) =>
            {
                cfg.ConstructServicesUsing(sp.GetService);
                cfg.AddProfile<GarmentCategoryProfile>();
                cfg.AddProfile<GarmentModelMappingProfile>();
                cfg.AddProfile<PointOfSaleMappingProfile>();
                cfg.AddProfile<OperationMappingProfile>();
                
            },
            typeof(GarmentCategoryProfile).Assembly);

        return services.BuildServiceProvider();
    }
}
