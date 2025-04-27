using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

public class EmployeeCreateViewModel
{
    private static readonly Dictionary<(bool hasRate, bool hasSalaryPercentage), EmployeeKind> KindMap =
        new()
        {
            [(false, false)] = EmployeeKind.Process,
            [(true, false)] = EmployeeKind.Rate,
            [(false, true)] = EmployeeKind.Technologist
        };
    public string Name { get; set; } = default!;
    public string InternalId { get; set; } = default!;
    public Department Department { get; set; }
    public decimal? Premium { get; set; }
    public decimal? Rate { get; set; }
    public int? SalaryPercentage { get; set; }

    public EmployeeKind GetEmployeeKind()
    {
        var hasRate = Rate.GetValueOrDefault() > 0;
        var hasSalaryPercentage = SalaryPercentage.GetValueOrDefault() > 0;

        if (KindMap.TryGetValue((hasRate, hasSalaryPercentage), out var kind))
        {
            return kind;
        }

        throw new SewingFactoryArgumentException(
            $"Invalid combination of Rate={Rate} and SalaryPercentage={SalaryPercentage}");
    }
}

public enum EmployeeKind
{
    Process,
    Rate,
    Technologist
}