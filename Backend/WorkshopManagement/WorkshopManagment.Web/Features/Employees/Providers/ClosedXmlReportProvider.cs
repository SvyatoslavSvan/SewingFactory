using ClosedXML.Excel;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Providers;

public sealed class ClosedXmlReportProvider : IReportProvider
{
    public Task<byte[]> GetReportAsync(IList<Salary> salaries)
    {
        if (salaries is null || salaries.Count == 0)
        {
            throw new SewingFactoryArgumentException("List of salaries is empty", nameof(salaries));
        }

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Report");

        WriteHeader(ws);
        WriteBody(ws, salaries, out var lastRow);
        WriteTotals(ws, salaries, lastRow + 1);

        using var ms = new MemoryStream();
        wb.SaveAs(ms);

        return Task.FromResult(ms.ToArray());
    }

    #region Header

    private static void WriteHeader(IXLWorksheet ws)
    {
        ws.Column(2).Width = 50;

        ws.Range("A1:G1").Merge().Value = "Зведена відомість";
        Center(ws, "A1:G1");

        ws.Range("A3:A4").Merge().Value = "Таб №";
        ws.Range("B3:B4").Merge().Value = "Працівник";

        ws.Range("C3:F3").Merge().Value = "Нараховано";
        ws.Cell("C4").Value = "Оплата";
        ws.Cell("D4").Value = "Премія";
        ws.Cell("E4").Value = "Додат.";
        ws.Cell("F4").Value = "Усього";

        ws.Range("G3:G4").Merge().Value = "На руки";

        Center(ws, "A3:G4");
    }

    #endregion

    #region Body & Totals

    private static void WriteBody(IXLWorksheet ws, IList<Salary> s, out int row)
    {
        row = 5;
        foreach (var sal in s)
        {
            ws.Cell(row, 1).Value = sal.Employee.InternalId;
            ws.Cell(row, 2).Value = sal.Employee.Name;

            ws.Cell(row, 3).Value = Round(sal.Payment);
            ws.Cell(row, 4).Value = Round(sal.Premium);
            ws.Cell(row, 5).Value = Round(sal.AdditionalPayment);

            var total = sal.Payment + sal.Premium + sal.AdditionalPayment;
            ws.Cell(row, 6).Value = Round(total);
            ws.Cell(row, 7).Value = Round(total);

            row++;
        }
    }

    private static void WriteTotals(IXLWorksheet ws, IList<Salary> s, int row)
    {
        var pay = s.Sum(selector: x => x.Payment.Amount);
        var prem = s.Sum(selector: x => x.Premium.Amount);
        var add = s.Sum(selector: x => x.AdditionalPayment.Amount);
        var all = pay + prem + add;

        ws.Cell(row, 1).Value = "Разом";
        ws.Cell(row, 3).Value = Round(pay);
        ws.Cell(row, 4).Value = Round(prem);
        ws.Cell(row, 5).Value = Round(add);
        ws.Cell(row, 6).Value = Round(all);
        ws.Cell(row, 7).Value = Round(all);
    }

    #endregion

    // ─────────────────────────

    #region Helpers

    private static decimal Round(Money m) => Round(m.Amount);

    private static decimal Round(decimal v) =>
        Math.Round(v, 2, MidpointRounding.AwayFromZero);

    private static void Center(IXLWorksheet ws, string range)
    {
        var rng = ws.Range(range);
        rng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rng.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
    }

    #endregion
}