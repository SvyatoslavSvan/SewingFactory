using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Repeats;

public class ReadEmployeeTaskRepeatViewModel : IIdentityViewModel
{
    public required EmployeeReadViewModel Employees { get; set; }
    public int Repeats { get; set; }
    public Guid Id { get; set; }
}