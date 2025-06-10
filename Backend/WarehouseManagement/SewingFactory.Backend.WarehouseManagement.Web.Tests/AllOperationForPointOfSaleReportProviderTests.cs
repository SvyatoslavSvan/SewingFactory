using ClosedXML.Excel;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Implementations;
using SewingFactory.Common.Domain.ValueObjects;
using System.Globalization;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public sealed class AllOperationForPointOfSaleReportProviderTests
{
    private static Money Uah(decimal amount) => new(amount);

    private static (PointOfSale pos, DateRange range, decimal inc, decimal outc) Arrange()
    {
        var cat = new GarmentCategory("Футболки", []);
        var model = new GarmentModel("F-100", cat, Uah(250));
        var pos1 = new PointOfSale("ТТ-1");
        var pos2 = new PointOfSale("ТТ-2");

        pos1.Receive(model, 20, new DateOnly(2025, 6, 1));
        pos1.Sell(model.Id, 3, new DateOnly(2025, 6, 5)); // -750
        pos1.WriteOff(model.Id, 1, new DateOnly(2025, 6, 5)); // -250
        pos1.Receive(model, 5, new DateOnly(2025, 6, 6)); // +1250
        pos1.Transfer(model, 2, new DateOnly(2025, 6, 7), pos2); // -500

        var range = new DateRange(new DateOnly(2025, 6, 5), new DateOnly(2025, 6, 7));
        var inc = 1250m;
        var outc = 1500m; // 750 + 250 + 500

        return (pos1, range, inc, outc);
    }

    [Fact]
    public void WorksheetName_ShouldBe_Register()
    {
        var (pos, r, _, _) = Arrange();
        var sut = new AllOperationForPointOfSaleReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, r)));
        Assert.Single(wb.Worksheets);
        Assert.Equal("Реєстр", wb.Worksheet(1).Name);
    }

    [Fact]
    public void Header_ShouldContain_PointOfSale_And_Period()
    {
        var (pos, r, _, _) = Arrange();
        var sut = new AllOperationForPointOfSaleReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, r)));
        var ws = wb.Worksheet(1);

        Assert.Equal($"Реєстр з торгової точки \"{pos.Name}\"", ws.Cell(1, 1).GetString());
        Assert.Equal($"За період з {RuDate(r.Start)} по {RuDate(r.End)}", ws.Cell(2, 1).GetString());
    }

    [Fact]
    public void Body_ShouldAggregate_Sums_Correctly()
    {
        var (pos, range, expectedIncome, expectedOutcome) = Arrange();
        var sut = new AllOperationForPointOfSaleReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, range)));
        var ws = wb.Worksheet(1);
        var usedRows = ws.RangeUsed()!.RowsUsed().ToList();
        var dataAndTotals = usedRows.Skip(3).ToList();
        var bodyRows = dataAndTotals.Take(dataAndTotals.Count - 1).ToList();
        Assert.Equal(4, bodyRows.Count);
        var income = bodyRows.Sum(selector: r => r.Cell(3).GetValue<decimal?>() ?? 0m);
        var outcome = bodyRows.Sum(selector: r => r.Cell(4).GetValue<decimal?>() ?? 0m);

        Assert.Equal(expectedIncome, income);
        Assert.Equal(expectedOutcome, outcome);
    }


    [Fact]
    public void TotalsRow_ShouldMatch_Summary()
    {
        var (pos, r, inc, outc) = Arrange();
        var sut = new AllOperationForPointOfSaleReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, r)));
        var ws = wb.Worksheet(1);
        var last = ws.RangeUsed()!.LastRow().RowNumber();

        Assert.Equal("Разом:", ws.Cell(last, 1).GetString());
        Assert.Equal(inc, ws.Cell(last, 3).GetValue<decimal>());
        Assert.Equal(outc, ws.Cell(last, 4).GetValue<decimal>());
    }

    private static string RuDate(DateOnly d) =>
        d.ToDateTime(TimeOnly.MinValue)
            .ToString("dd MMMM yyyy 'г.'", new CultureInfo("ru-RU"));
}
