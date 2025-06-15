using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;

public sealed class GetForUpdateWorkshopDocumentViewModel
{
    public List<EmployeeReadViewModel> Employees { get; set; } = null!;
}