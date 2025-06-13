using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Mapping;

public class TimesheetMappingProfile : Profile
{
    public TimesheetMappingProfile()
    {
        CreateMap<Timesheet, TimesheetViewModel>()
            .ForMember(destinationMember: d => d.Date,
                memberOptions: o => o.MapFrom(mapExpression: s => s.Date))
            .ForMember(destinationMember: d => d.DaysCount,
                memberOptions: o => o.MapFrom(mapExpression: s => s.DaysCount))
            .ForMember(destinationMember: d => d.Employees,
                memberOptions: o => o.MapFrom(mapExpression: s =>
                    s.WorkDays
                        .GroupBy(w => w.Employee)
                        .Select(g => g.Key)));

        CreateMap<RateBasedEmployee, TimesheetEmployeeViewModel>()
            .ForMember(destinationMember: d => d.EmployeeName,
                memberOptions: o => o.MapFrom(mapExpression: e => e.Name))
            .ForMember(destinationMember: d => d.WorkDays,
                memberOptions: o => o.MapFrom(mappingFunction: (
                        e,
                        _,
                        _,
                        ctx) =>
                    ((Timesheet)ctx.Items["Timesheet"])
                    .WorkDays
                    .Where(predicate: w => w.Employee.Id == e.Id)));

        CreateMap<WorkDay, ReadWorkDayViewModel>()
            .ForMember(destinationMember: d => d.Date,
                memberOptions: o => o.MapFrom(mapExpression: w => w.Date))
            .ForMember(destinationMember: d => d.Hours,
                memberOptions: o => o.MapFrom(mapExpression: w => w.Hours))
            .ForMember(destinationMember: x => x.Id,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Id));

        CreateMap<UpdateWorkdayViewModel, WorkDay>()
            .ForMember(destinationMember: d => d.Hours,
                memberOptions: o => o.MapFrom(mapExpression: w => w.Hours))
            .ForMember(destinationMember: x => x.Date,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: x => x.Employee,
                memberOptions: opt => opt.Ignore());
    }
}