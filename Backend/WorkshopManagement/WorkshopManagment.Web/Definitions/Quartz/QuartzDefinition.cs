using Calabonga.AspNetCore.AppDefinitions;
using Quartz;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.QuartzJobs;
using System.Runtime.InteropServices;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.Quartz;

public class QuartzDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(configure: q =>
        {
            var jobKey = new JobKey(nameof(GenerateMonthlyTimesheetJob));
            q.AddJob<GenerateMonthlyTimesheetJob>(configure: opts => opts
                .WithIdentity(jobKey)
                .StoreDurably()
            );

            q.AddTrigger(configure: opts => opts
                .ForJob(jobKey)
                .WithIdentity("GenerateMonthlyTimesheetTrigger")
                .WithCronSchedule(
                    "0 0 0 1 * ?",
                    action: x => x.InTimeZone(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time")
                        : TimeZoneInfo.FindSystemTimeZoneById("Europe/Kyiv"))
                )
            );
        });

        builder.Services.AddQuartzHostedService(configure: options =>
            options.WaitForJobsToComplete = true
        );
    }
}