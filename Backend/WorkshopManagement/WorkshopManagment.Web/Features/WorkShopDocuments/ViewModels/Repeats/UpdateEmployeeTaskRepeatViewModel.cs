using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Repeats;

public class UpdateEmployeeTaskRepeatViewModel : IIdentityViewModel
{
    public Guid EmployeeId { get; set; }
    public int RepeatsCount { get; set; }
    public Guid Id { get; set; }
}