using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Repeats;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Tasks;

public class UpdateWorkShopTaskViewModel : IIdentityViewModel
{
    public List<UpdateEmployeeTaskRepeatViewModel> EmployeeRepeats { get; set; } = null!;
    public Guid Id { get; set; }
}