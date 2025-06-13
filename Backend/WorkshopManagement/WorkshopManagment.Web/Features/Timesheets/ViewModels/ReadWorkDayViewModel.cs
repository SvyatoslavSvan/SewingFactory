using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.ViewModels;

public class ReadWorkDayViewModel : IIdentityViewModel
{
    public DateOnly Date { get; init; }
    public int Hours { get; init; }
    public Guid Id { get; set; }
}