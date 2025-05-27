using System.Reflection;
using System.Security.Claims;
using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public class GetLeftOversReportRequestHandlerTests
{
    private static void SetId(object entity, Guid id) // AAA
        => typeof(Identity)
           .GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
           .SetValue(entity, id);

    private static PointOfSale SeedPointOfSale(IServiceProvider sp, Guid id) // AAA
    {
        var pants = new GarmentCategory("Pants", []);
        var model = new GarmentModel("Demo", pants, new Money(10m));

        SetId(pants, Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        SetId(model, Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

        var pos = new PointOfSale("Store #1");
        SetId(pos, id);

        pos.Receive(model, 1, DateOnly.FromDateTime(DateTime.Today));

        var ctx = sp.GetRequiredService<ApplicationDbContext>();
        ctx.AddRange(pants, model, pos);
        ctx.SaveChanges();
        return pos;
    }

    private sealed class StubReportProvider(byte[] bytes) : ILeftoverReportProvider
    {
        public byte[] Build(PointOfSale _) => bytes;
    }

    [Fact(DisplayName = "Handle: returns file when POS exists")]
    public async Task Handle_PosExists_ReturnsFile()
    {
        // Arrange
        var sp = TestHelpers.BuildServices();
        var id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        _ = SeedPointOfSale(sp, id);
        var uow = sp.GetRequiredService<IUnitOfWork>();
        var file = new byte[] { 1, 2, 3 };
        var sut = new GetLeftOversReportRequestHandler(uow, new StubReportProvider(file));
        var req = new GetLeftOversReportRequest(new ClaimsPrincipal(), id);

        // Act
        var res = await sut.Handle(req, CancellationToken.None);

        // Assert
        var fileResult = Assert.IsType<FileContentHttpResult>(res);
        Assert.Equal(file, fileResult.FileContents);
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
        Assert.EndsWith(".xlsx", fileResult.FileDownloadName);
    }

    [Fact(DisplayName = "Handle: returns 404 when POS absent")]
    public async Task Handle_PosMissing_ReturnsNotFound()
    {
        // Arrange
        var sp = TestHelpers.BuildServices();
        var uow = sp.GetRequiredService<IUnitOfWork>();
        var sut = new GetLeftOversReportRequestHandler(uow, new StubReportProvider([]));
        var id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var req = new GetLeftOversReportRequest(new ClaimsPrincipal(), id);

        // Act
        var res = await sut.Handle(req, CancellationToken.None);

        // Assert
        var notFound = Assert.IsType<NotFound<string>>(res);
        Assert.Contains(id.ToString(), notFound.Value, StringComparison.Ordinal);
    }
}
