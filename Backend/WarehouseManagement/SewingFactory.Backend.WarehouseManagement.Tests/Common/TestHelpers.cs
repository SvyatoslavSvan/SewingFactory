using Calabonga.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Mapping.Converters;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Mapping.Profiles;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Mapping;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Mapping.Profiles;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Implementations;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;

namespace SewingFactory.Backend.WarehouseManagement.Tests.Common;

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
    
    static Mock<ConsumeContext<T>> CreateContext<T>(T message) where T : class
    {
        var ctx = new Mock<ConsumeContext<T>>();
        ctx.SetupGet(c => c.Message).Returns(message);
        ctx.SetupGet(c => c.CancellationToken).Returns(CancellationToken.None);
        return ctx;
    }
}
