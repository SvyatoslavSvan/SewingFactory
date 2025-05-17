namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Providers;

public interface IDateTimeProvider
{
    public DateOnly CurrentMonthStart { get;}
}