using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Repeats;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Tasks;

public class ReadWorkshopTaskViewModel : IIdentityViewModel
{
    public ReadProcessViewModel Process { get; set; } = null!;
    public List<ReadEmployeeTaskRepeatViewModel>? EmployeeRepeats { get; set; } = null!;
    public Guid Id { get; set; }
}