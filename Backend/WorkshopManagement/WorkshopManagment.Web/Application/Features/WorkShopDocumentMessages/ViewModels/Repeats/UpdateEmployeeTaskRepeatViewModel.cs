using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Repeats;

public class UpdateEmployeeTaskRepeatViewModel : IIdentityViewModel
{
    public Guid EmployeeId { get; set; }
    public int RepeatsCount { get; set; }
    public Guid Id { get; set; }
}