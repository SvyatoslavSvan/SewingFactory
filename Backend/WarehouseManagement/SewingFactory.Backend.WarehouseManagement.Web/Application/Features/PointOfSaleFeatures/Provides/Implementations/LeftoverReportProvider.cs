using ClosedXML.Excel;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;
using TimeZoneConverter;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Implementations;

public sealed class LeftoverReportProvider : ILeftoverReportProvider
{
    public byte[] Build(PointOfSale pointOfSale)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Звіт");

        worksheet.Cell(1, 1).Value = $"Звіт по точці продажу: {pointOfSale.Name}";
        worksheet.Range(1, 1, 1, 5).Merge().Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        var kyivNow = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TZConvert.GetTimeZoneInfo("Europe/Kyiv"));

        worksheet.Cell(2, 1).Value = $"Дата: {DateOnly.FromDateTime(kyivNow):dd.MM.yyyy}";
        worksheet.Range(2, 1, 2, 5).Merge().Style.Alignment
            .SetHorizontal(XLAlignmentHorizontalValues.Center);

        var headers = new[] { "Модель", "Ціна (₴)", "Кількість", "Сума", "Пересорт" };
        for (var i = 0; i < headers.Length; i++)
        {
            var c = worksheet.Cell(4, i + 1);
            c.Value = headers[i];
            c.Style.Font.SetBold();
            c.Style.Fill.BackgroundColor = XLColor.SkyBlue;
        }

        const int width = 5;

        var row = 5;
        var groups = pointOfSale.StockItems
            .OrderBy(keySelector: s => s.GarmentModel.Category.Name)
            .ThenBy(keySelector: s => s.GarmentModel.Name)
            .GroupBy(keySelector: s => s.GarmentModel.Category.Name);

        foreach (var group in groups)
        {
            worksheet.Range(row, 1, row, width).Merge()
                .SetValue(group.Key)
                .Style.Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.LightGray);

            row++;

            foreach (var stockItem in group)
            {
                worksheet.Cell(row, 1).Value = stockItem.GarmentModel.Name;
                worksheet.Cell(row, 2).Value = stockItem.GarmentModel.Price.Amount;
                worksheet.Cell(row, 3).Value = stockItem.Quantity;
                worksheet.Cell(row, 4).Value = stockItem.SumOfStock;
                worksheet.Cell(row, 5).Value = stockItem.ShortageQuantity;

                if (stockItem.ShortageQuantity != 0)
                {
                    worksheet.Range(row, 1, row, width)
                        .Style.Fill.BackgroundColor = XLColor.LightCoral;
                }

                row++;
            }

            var categorySum = group.Sum(selector: si => si.SumOfStock);
            worksheet.Range(row, 1, row, width - 2).Merge()
                .SetValue("Разом по категорії:")
                .Style.Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            worksheet.Cell(row, width - 1).Value = categorySum;
            worksheet.Cell(row, width - 1).Style.Font.SetBold();

            row += 2;
        }

        worksheet.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);

        return ms.ToArray();
    }
}
