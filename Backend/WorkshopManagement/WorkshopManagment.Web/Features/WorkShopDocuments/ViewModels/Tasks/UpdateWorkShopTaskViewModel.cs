using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Repeats;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Tasks;

public class UpdateWorkShopTaskViewModel : IIdentityViewModel
{
    public List<UpdateEmployeeTaskRepeatViewModel> EmployeeRepeats { get; set; } = null!;
    public Guid Id { get; set; }
}