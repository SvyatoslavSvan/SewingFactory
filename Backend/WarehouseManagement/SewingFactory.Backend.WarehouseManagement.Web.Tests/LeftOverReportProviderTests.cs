using ClosedXML.Excel;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.ValueObjects;
using System.Reflection;
using TimeZoneConverter;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public class LeftoverReportProviderTests
{
    private static void SetId(object entity, Guid id)
        => typeof(Identity)
            .GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(entity, id);

    private static PointOfSale CreatePointOfSale() 
    {
        var pants = new GarmentCategory("Pants", []);
        var winterPants = new GarmentModel("Winter Pants", pants, new Money(1_200m));
        var summerPants = new GarmentModel("Summer Pants", pants, new Money(900m));

        var shirts = new GarmentCategory("Shirts", []);
        var linenShirt = new GarmentModel("Linen Shirt", shirts, new Money(750m));

        SetId(pants, Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        SetId(winterPants, Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
        SetId(summerPants, Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));
        SetId(shirts, Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));
        SetId(linenShirt, Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

        var pos = new PointOfSale("Store #1");
        SetId(pos, Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"));

        pos.Receive(winterPants, 5, DateOnly.FromDateTime(DateTime.Today));
        pos.Receive(summerPants, 10, DateOnly.FromDateTime(DateTime.Today));
        pos.Receive(linenShirt, 3, DateOnly.FromDateTime(DateTime.Today));

        pos.StockItems.First(predicate: s => s.GarmentModel.Id == linenShirt.Id).ReduceQuantity(5);

        return pos;
    }

    [Fact(DisplayName = "Build: header, date and columns are correct")]
    public void Build_HeaderDateAndColumnsAreCorrect()
    {
        // Arrange
        var provider = new LeftoverReportProvider();
        var pos = CreatePointOfSale();

        // Act
        var bytes = provider.Build(pos);

        // Assert
        using var ms = new MemoryStream(bytes);
        using var wb = new XLWorkbook(ms);
        var ws = wb.Worksheet("Звіт");

        Assert.NotNull(ws);
        Assert.Equal($"Звіт по точці продажу: {pos.Name}", ws.Cell(1, 1).GetString());

        var expectedDate = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TZConvert.GetTimeZoneInfo("Europe/Kyiv"))
            .ToString("dd.MM.yyyy");

        Assert.Contains(expectedDate, ws.Cell(2, 1).GetString());

        var titles = new[] { "Модель", "Ціна (₴)", "Кількість", "Сума", "Пересорт" };
        for (var i = 0; i < titles.Length; i++)
        {
            Assert.Equal(titles[i], ws.Cell(4, i + 1).GetString());
        }
    }

    [Fact(DisplayName = "Build: items grouped and category total calculated")]
    public void Build_GroupingAndCategoryTotal_Correct()
    {
        // Arrange
        var provider = new LeftoverReportProvider();
        var pos = CreatePointOfSale();

        // Act
        var bytes = provider.Build(pos);

        // Assert
        using var ms = new MemoryStream(bytes);
        using var wb = new XLWorkbook(ms);
        var ws = wb.Worksheet("Звіт");

        var pantsHeader = ws.CellsUsed().First(predicate: c => c.GetString() == "Pants");
        var headerRow = pantsHeader.Address.RowNumber;

        var summerRow = headerRow + 1;
        var winterRow = headerRow + 2;
        var totalRow = headerRow + 3;

        const decimal expectedSum = (10 * 900m) + (5 * 1_200m);

        Assert.Equal("Summer Pants", ws.Cell(summerRow, 1).GetString());
        Assert.Equal("Winter Pants", ws.Cell(winterRow, 1).GetString());
        Assert.Equal("Разом по категорії:", ws.Cell(totalRow, 1).GetString());
        Assert.Equal(expectedSum, ws.Cell(totalRow, 4).GetValue<decimal>());
    }

    [Fact(DisplayName = "Build: shortage rows highlighted")]
    public void Build_ShortageRows_Highlighted()
    {
        // Arrange
        var provider = new LeftoverReportProvider();
        var pos = CreatePointOfSale();

        // Act
        var bytes = provider.Build(pos);

        // Assert
        using var ms = new MemoryStream(bytes);
        using var wb = new XLWorkbook(ms);
        var ws = wb.Worksheet("Звіт");

        var shortageCell = ws.CellsUsed()
            .First(predicate: c => c.Address.ColumnNumber == 5 &&
                                   c.DataType == XLDataType.Number &&
                                   c.GetDouble() != 0);

        var row = ws.Row(shortageCell.Address.RowNumber);
        Assert.Equal(XLColor.LightCoral, row.Cells().First().Style.Fill.BackgroundColor);
    }
}
