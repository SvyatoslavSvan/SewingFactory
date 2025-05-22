using TimeZoneConverter;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Providers
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        private static readonly TimeZoneInfo _kyivZone =
            TZConvert.GetTimeZoneInfo("Europe/Kyiv");

        public DateOnly CurrentMonthStart
        {
            get
            {
                var nowKiev = TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.UtcNow,
                    _kyivZone);
                return new DateOnly(nowKiev.Year, nowKiev.Month, 1);
            }
        }
    }
}
