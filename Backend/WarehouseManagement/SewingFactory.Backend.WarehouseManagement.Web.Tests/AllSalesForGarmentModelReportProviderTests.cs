using ClosedXML.Excel;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Implementations;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public sealed class AllSalesForGarmentModelReportProviderTests
{
    private static Money Amount(decimal a) => new(a); 

    private static (GarmentModel model,
        PointOfSale pos1, PointOfSale pos2,
        SaleOperation[] allSales,
        DateRange range,
        decimal pos1Sum, decimal pos2Sum) Arrange()
    {
        var cat = new GarmentCategory("Футболки", []);
        var model = new GarmentModel("F-100", cat, Amount(150));

        var pos1 = new PointOfSale("ТТ-1");
        var pos2 = new PointOfSale("ТТ-2");

        pos1.Receive(model, 10, new DateOnly(2025, 6, 1));
        pos2.Receive(model, 5, new DateOnly(2025, 6, 1));

        pos1.Sell(model.Id, 3, new DateOnly(2025, 6, 2)); // 450
        pos1.Sell(model.Id, 2, new DateOnly(2025, 6, 3)); // 300
        pos2.Sell(model.Id, 1, new DateOnly(2025, 6, 4)); // 150

        var range = new DateRange(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 5));

        var pos1Sum = 450m + 300m; // 750
        var pos2Sum = 150m;

        var allSales = pos1.Operations.OfType<SaleOperation>()
            .Concat(pos2.Operations.OfType<SaleOperation>())
            .ToArray();

        return (model, pos1, pos2, allSales, range, pos1Sum, pos2Sum);
    }

    [Fact]
    public void Worksheet_Should_Be_Named_Report()
    {
        var (model, pos1, pos2, sales, range, _, _) = Arrange();
        var sut = new AllSalesForGarmentModelReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(sales, range)));
        Assert.Single(wb.Worksheets);
        Assert.Equal("Report", wb.Worksheet(1).Name);
    }

    [Fact]
    public void Header_And_SubHeader_Should_Be_Correct()
    {
        var (model, _, _, sales, range, _, _) = Arrange();
        var sut = new AllSalesForGarmentModelReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(sales, range)));
        var ws = wb.Worksheet(1);

        Assert.Equal($"Продажі по продукції \"{model.Name}\"", ws.Cell(1, 1).GetString());
        Assert.Equal($"За період з {range.Start:dd MMM yyyy} по {range.End:dd MMM yyyy}",
            ws.Cell(2, 1).GetString());
    }

    [Fact]
    public void Column_Titles_Should_Match_Specification()
    {
        var (_, _, _, sales, range, _, _) = Arrange();
        var sut = new AllSalesForGarmentModelReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(sales, range)));
        var ws = wb.Worksheet(1);

        string[] expected = { "Дата", "Кількість", "Ціна", "Сума" };
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], ws.Cell(4, i + 1).GetString());
        }
    }

    [Fact]
    public void Groups_And_Totals_Should_Be_Correct()
    {
        var (model, pos1, pos2, sales, range, pos1Sum, pos2Sum) = Arrange();
        var sut = new AllSalesForGarmentModelReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(sales, range)));
        var ws = wb.Worksheet(1);

        var row = 5;

        Assert.Equal(pos1.Name, ws.Cell(row, 1).GetString());
        row++; 

        Assert.Equal("02.06.25", ws.Cell(row, 1).GetString()); // дата
        Assert.Equal(3, ws.Cell(row, 2).GetValue<int>());
        Assert.Equal(150m, ws.Cell(row, 3).GetValue<decimal>());
        Assert.Equal(450m, ws.Cell(row, 4).GetValue<decimal>());
        row++;

        Assert.Equal("03.06.25", ws.Cell(row, 1).GetString());
        Assert.Equal(2, ws.Cell(row, 2).GetValue<int>());
        Assert.Equal(150m, ws.Cell(row, 3).GetValue<decimal>());
        Assert.Equal(300m, ws.Cell(row, 4).GetValue<decimal>());
        row++;

        Assert.Equal("Разом:", ws.Cell(row, 1).GetString());
        Assert.Equal(pos1Sum, ws.Cell(row, 4).GetValue<decimal>());
        row += 2; 

        Assert.Equal(pos2.Name, ws.Cell(row, 1).GetString());
        row++;

        Assert.Equal("04.06.25", ws.Cell(row, 1).GetString());
        Assert.Equal(1, ws.Cell(row, 2).GetValue<int>());
        Assert.Equal(150m, ws.Cell(row, 3).GetValue<decimal>());
        Assert.Equal(150m, ws.Cell(row, 4).GetValue<decimal>());
        row++;
        Assert.Equal("Разом:", ws.Cell(row, 1).GetString());
        Assert.Equal(pos2Sum, ws.Cell(row, 4).GetValue<decimal>());
    }
}
