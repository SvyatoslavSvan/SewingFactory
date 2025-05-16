using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Repeats
{
    public class UpdateEmployeeTaskRepeatViewModel :IIdentityViewModel
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int RepeatsCount { get; set; }
    }
}
