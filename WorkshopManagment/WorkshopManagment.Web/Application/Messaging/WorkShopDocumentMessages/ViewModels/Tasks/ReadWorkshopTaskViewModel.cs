using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Repeats;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Tasks
{
    public class ReadWorkshopTaskViewModel : IIdentityViewModel
    {
        public Guid Id { get; set; }
        public ReadProcessViewModel Process { get; set; } = null!;
        public List<ReadEmployeeTaskRepeatViewModel>? EmployeeRepeats { get; set; } = null!;
    }
}
