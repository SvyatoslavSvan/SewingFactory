using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Mapping
{
    public class TimesheetMappingProfile : Profile
    {
        public TimesheetMappingProfile()
        {
            CreateMap<Timesheet, TimesheetViewModel>()
                .ForMember(d => d.Date,
                    o => o.MapFrom(s => s.Date))
                .ForMember(d => d.DaysCount,
                    o => o.MapFrom(s => s.DaysCount))
                .ForMember(d => d.Employees,
                    o => o.MapFrom(s =>
                        s.WorkDays
                            .GroupBy(w => w.Employee)
                            .Select(g => g.Key)));

            CreateMap<RateBasedEmployee, TimesheetEmployeeViewModel>()
                .ForMember(d => d.EmployeeName,
                    o => o.MapFrom(e => e.Name))
                .ForMember(d => d.WorkDays,
                    o => o.MapFrom((
                            e,
                            _,
                            _,
                            ctx) =>
                        ((Timesheet)ctx.Items["Timesheet"])
                        .WorkDays
                        .Where(w => w.Employee.Id == e.Id)));

            CreateMap<WorkDay, ReadWorkDayViewModel>()
                .ForMember(d => d.Date,
                    o => o.MapFrom(w => w.Date))
                .ForMember(d => d.Hours,
                    o => o.MapFrom(w => w.Hours))
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(x => x.Id));

            CreateMap<UpdateWorkdayViewModel, WorkDay>()
                .ForMember(d => d.Hours,
                    o => o.MapFrom(w => w.Hours))
                .ForMember(x => x.Date,
                    opt => opt.Ignore())
                .ForMember(x => x.Employee,
                    opt => opt.Ignore());
        }
    }
}