using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Repeats;

public class ReadEmployeeTaskRepeatViewModel : IIdentityViewModel
{
    public required EmployeeReadViewModel Employees { get; set; }
    public int Repeats { get; set; }
    public Guid Id { get; set; }
}