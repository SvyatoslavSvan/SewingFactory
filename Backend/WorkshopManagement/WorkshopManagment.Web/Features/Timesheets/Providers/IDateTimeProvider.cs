namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Providers;

public interface IDateTimeProvider
{
    public DateOnly CurrentMonthStart { get; }
}