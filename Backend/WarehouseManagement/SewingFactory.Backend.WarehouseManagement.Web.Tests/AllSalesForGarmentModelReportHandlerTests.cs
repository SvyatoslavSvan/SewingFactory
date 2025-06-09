using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public sealed class AllSalesForGarmentModelReportHandlerTests
{
    [Fact]
    public async Task Sales_Handler_Returns_File()
    {
        var sp = TestHelpers.BuildServices();
        using var scope = sp.CreateScope();

        var uow      = scope.ServiceProvider.GetRequiredService<Calabonga.UnitOfWork.IUnitOfWork>();
        var provider = scope.ServiceProvider.GetRequiredService<IAllSalesForGarmentModelReportProvider>();

        var cat   = new GarmentCategory("Cat", []);
        var model = new GarmentModel("Jacket", cat, new Money(300));

        var pos1 = new PointOfSale("POS-C");
        pos1.Receive(model, 4, new DateOnly(2025, 6, 1));
        pos1.Sell   (model.Id, 2, new DateOnly(2025, 6, 4));   

        await uow.GetRepository<PointOfSale>().InsertAsync(pos1);
        await uow.SaveChangesAsync();

        var range   = new DateRange(new(2025, 6, 1), new(2025, 6, 10));
        var handler = new AllSalesForGarmentModelReportRequestHandler(provider, uow);

        var request = new AllSalesForGarmentModelReportRequest(
            new System.Security.Claims.ClaimsPrincipal(), range, model.Id);

        var result = await handler.Handle(request, CancellationToken.None);

        var file = Assert.IsType<FileContentHttpResult>(result);
        Assert.True(file.FileContents.Length > 0);
        Assert.Contains(model.Name, file.FileDownloadName);
    }
}
