using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Repeats
{
    public class ReadEmployeeTaskRepeatViewModel : IIdentityViewModel
    {
        public Guid Id { get; set; }
        public required EmployeeReadViewModel Employees { get; set; }
        public int Repeats { get; set; }
    }
}
