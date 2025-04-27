using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages;

public class ProcessBasedEmployeeProfile : Profile
{
    public ProcessBasedEmployeeProfile()
    {
        CreateMap<ProcessBasedEmployeeViewModel, ProcessBasedEmployee>()
            .ConstructUsing(ctor: src => new ProcessBasedEmployee(src.Name, src.InternalId, src.Department, new Percent(src.Premium)))
            .ForMember(destinationMember: x => x.Documents, memberOptions: opt => opt.Ignore());

        CreateMap<ProcessBasedEmployee, IdentityProcessBasedEmployeeViewModel>()
            .ForMember(destinationMember: dest => dest.Id, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Id))
            .ForPath(d => d.Employee.Premium, opt => opt.MapFrom(s => s.Premium.Value));

        CreateMap<IdentityProcessBasedEmployeeViewModel, ProcessBasedEmployee>()
            .ForMember(destinationMember: dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id))
            .ForMember(destinationMember: dest => dest.Premium, memberOptions: opt => opt.MapFrom(src => src.Employee.Premium))
            .ForMember(dest => dest.InternalId, opt => opt.MapFrom(x => x.Employee.InternalId))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Employee.Department))
            .ForMember(dest => dest.Name, opts => opts.MapFrom(x => x.Employee.Name))
            .ForMember(destinationMember: x => x.Documents, memberOptions: opt => opt.Ignore());

        CreateMap<Employee, EmployeeReadViewModel>()
            .Include<ProcessBasedEmployee, ProcessEmployeeReadViewModel>()
            .Include<RateBasedEmployee, RateEmployeeReadViewModel>()
            .Include<Technologist, TechnologistReadViewModel>();

        CreateMap<ProcessBasedEmployee, ProcessEmployeeReadViewModel>()
            .ForMember(d => d.Premium, o => o.MapFrom(s => s.Premium.Value));

        CreateMap<RateBasedEmployee, RateEmployeeReadViewModel>()
            .ForMember(d => d.Rate, o => o.MapFrom(s => s.Rate.Amount))
            .ForMember(d => d.PremiumPercent, o => o.MapFrom(s => (int)s.Premium.Value));

        CreateMap<Technologist, TechnologistReadViewModel>()
            .ForMember(d => d.SalaryPercentage,
                o => o.MapFrom(s => s.SalaryPercentage.Value));

        CreateMap<EmployeeCreateViewModel, ProcessBasedEmployee>()
            .ConstructUsing(src
                => new ProcessBasedEmployee(
                    src.Name,
                    src.InternalId,
                    src.Department,
                    new Percent(src.Premium!.Value)
                ))
            .ForMember(dest => dest.Documents, opt => opt.Ignore());

        CreateMap<EmployeeCreateViewModel, RateBasedEmployee>()
            .ConstructUsing(src
                => new RateBasedEmployee(
                    src.Name,
                    src.InternalId,
                    new Money(src.Rate!.Value),
                    src.Department,
                    src.SalaryPercentage!.Value
                )).ForMember(dest => dest.Timesheets, opt => opt.Ignore())
            .ForMember(dest => dest.Documents, opt => opt.Ignore());
        CreateMap<EmployeeCreateViewModel, Technologist>()
            .ConstructUsing(src
                => new Technologist(
                    src.Name,
                    src.InternalId,
                    new Percent(src.SalaryPercentage!.Value),
                    src.Department
                )).ForMember(dest => dest.SalaryPercentage, opt => opt.Ignore());

        CreateMap<EmployeeCreateViewModel, Employee>()
            .ConvertUsing((src, _, ctx) =>
            {
                return src.GetEmployeeKind() switch
                {
                    EmployeeKind.Process => ctx.Mapper.Map<ProcessBasedEmployee>(src),
                    EmployeeKind.Rate => ctx.Mapper.Map<RateBasedEmployee>(src),
                    EmployeeKind.Technologist => ctx.Mapper.Map<Technologist>(src),
                    _ => throw new ArgumentOutOfRangeException()
                };
            });
    }
}