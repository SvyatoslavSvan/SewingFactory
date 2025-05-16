using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document
{
    public class GetForUpdateWorkshopDocumentViewModel
    {
        public List<EmployeeReadViewModel> Employees { get; set; } = null!;
    }
}
