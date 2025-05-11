using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels
{
    public class DeleteDepartmentViewModel : IIdentityViewModel
    {
        public Guid Id { get; set; }
    }
}
