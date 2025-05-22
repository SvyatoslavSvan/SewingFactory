using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Repeats;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Tasks;

public class ReadWorkshopTaskViewModel : IIdentityViewModel
{
    public ReadProcessViewModel Process { get; set; } = null!;
    public List<ReadEmployeeTaskRepeatViewModel>? EmployeeRepeats { get; set; } = null!;
    public Guid Id { get; set; }
}