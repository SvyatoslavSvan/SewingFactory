using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public sealed class AllOperationForStockReportHandlerTests
{
    [Fact]
    public async Task Stock_Handler_Returns_File()
    {
        var sp = TestHelpers.BuildServices();
        using var scope = sp.CreateScope();

        var uow      = scope.ServiceProvider.GetRequiredService<Calabonga.UnitOfWork.IUnitOfWork>();
        var provider = scope.ServiceProvider.GetRequiredService<IAllOperationForStockReportProvider>();

        var cat   = new GarmentCategory("Cat", []);
        var model = new GarmentModel("Dress", cat, new Money(200));
        var pos   = new PointOfSale("POS-B");

        pos.Receive(model, 5, new DateOnly(2025, 6, 3));
        await uow.GetRepository<PointOfSale>().InsertAsync(pos);
        await uow.SaveChangesAsync();

        var range   = new DateRange(new(2025, 6, 1), new(2025, 6, 30));
        var handler = new AllOperationForStockReportRequestHandler(uow, provider);

        var request = new AllOperationForStockReportRequest(
            new System.Security.Claims.ClaimsPrincipal(), range, pos.Id, model.Id);

        var result = await handler.Handle(request, CancellationToken.None);

        var file = Assert.IsType<FileContentHttpResult>(result);
        Assert.True(file.FileContents.Length > 0);
        Assert.Contains(model.Name, file.FileDownloadName);
    }
}
