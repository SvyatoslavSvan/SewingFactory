using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Mapping.Converters;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Mapping.Profiles;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Mapping;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Mapping.Profiles;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Implementations;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;

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
