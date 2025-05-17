using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;

public class GetForUpdateWorkshopDocumentViewModel
{
    public List<EmployeeReadViewModel> Employees { get; set; } = null!;
}