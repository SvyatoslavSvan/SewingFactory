using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public sealed class AllOperationForPointOfSaleReportHandler
{
    [Fact]
    public async Task Handler_Returns_File_With_Real_Data()
    {
        var sp = TestHelpers.BuildServices();
        using var scope = sp.CreateScope();

        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var provider = scope.ServiceProvider.GetRequiredService<IAllOperationForPointOfSaleReportProvider>();

        var cat = new GarmentCategory("Cat", []);
        var model = new GarmentModel("T-Shirt", cat, new Money(120));
        var pos = new PointOfSale("POS-A");

        pos.Receive(model, 10, new DateOnly(2025, 6, 2));
        await uow.GetRepository<PointOfSale>().InsertAsync(pos);
        await uow.SaveChangesAsync();

        var range = new DateRange(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));
        var handler = new AllOperationForPointOfSaleReportRequestHandler(provider, uow);

        var request = new AllOperationForPointOfSaleReportRequest(
            new ClaimsPrincipal(), pos.Id, range);

        var result = await handler.Handle(request, CancellationToken.None);

        var file = Assert.IsType<FileContentHttpResult>(result);
        Assert.True(file.FileContents.Length > 0);
        Assert.Contains(pos.Name, file.FileDownloadName);
    }

    [Fact]
    public async Task Handler_Returns_NotFound_When_Pos_Missing()
    {
        var sp = TestHelpers.BuildServices();
        using var scope = sp.CreateScope();

        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var provider = scope.ServiceProvider.GetRequiredService<IAllOperationForPointOfSaleReportProvider>();

        var range = new DateRange(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));
        var handler = new AllOperationForPointOfSaleReportRequestHandler(provider, uow);

        var request = new AllOperationForPointOfSaleReportRequest(
            new ClaimsPrincipal(), Guid.NewGuid(), range);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.IsType<NotFound<string>>(result);
    }
}
