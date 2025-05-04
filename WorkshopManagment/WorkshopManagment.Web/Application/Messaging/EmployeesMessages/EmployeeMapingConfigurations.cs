using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages;

public class EmployeeMappingConfiguration : Profile
{
    public EmployeeMappingConfiguration()
    {
        /* ---------- READ MAPPINGS ---------- */

        // map base Employee → flat ReadViewModel
        CreateMap<Employee, EmployeeReadViewModel>(MemberList.None)
            .ForMember(destinationMember: d => d.Id, memberOptions: o => o.MapFrom(mapExpression: s => s.Id))
            .ForMember(destinationMember: d => d.Name, memberOptions: o => o.MapFrom(mapExpression: s => s.Name))
            .ForMember(destinationMember: d => d.InternalId, memberOptions: o => o.MapFrom(mapExpression: s => s.InternalId))
            .ForMember(destinationMember: d => d.Department, memberOptions: o => o.MapFrom(mapExpression: s => s.Department));

        // ProcessBasedEmployee → adds Premium
        CreateMap<ProcessBasedEmployee, ProcessEmployeeReadViewModel>(MemberList.None)
            .IncludeBase<Employee, EmployeeReadViewModel>()
            .ForMember(destinationMember: d => d.Premium, memberOptions: o => o.MapFrom(mapExpression: s => s.Premium.Value));

        // RateBasedEmployee → adds Rate (in addition to inherited Premium)
        CreateMap<RateBasedEmployee, RateEmployeeReadViewModel>(MemberList.None)
            .IncludeBase<ProcessBasedEmployee, ProcessEmployeeReadViewModel>()
            .ForMember(destinationMember: d => d.Rate, memberOptions: o => o.MapFrom(mapExpression: s => s.Rate.Amount));

        // Technologist → adds SalaryPercentage
        CreateMap<Technologist, TechnologistReadViewModel>(MemberList.None)
            .IncludeBase<Employee, EmployeeReadViewModel>()
            .ForMember(destinationMember: d => d.SalaryPercentage,
                memberOptions: o => o.MapFrom(mapExpression: s => s.SalaryPercentage.Value));

        /* ---------- CREATE MAPPINGS (unchanged) ---------- */

        CreateMap<EmployeeCreateViewModel, Employee>()
            .ConstructUsing(ctor: (src, ctx) => src switch
            {
                RateEmployeeCreateViewModel r => ctx.Mapper.Map<RateBasedEmployee>(r),
                ProcessEmployeeCreateViewModel p => ctx.Mapper.Map<ProcessBasedEmployee>(p),
                TechnologistCreateViewModel t => ctx.Mapper.Map<Technologist>(t),
                _ => throw new SewingFactoryArgumentOutOfRangeException(nameof(src),
                    $"Unknown create DTO type: {src.GetType().Name}")
            });

        CreateMap<ProcessEmployeeCreateViewModel, ProcessBasedEmployee>()
            .ConstructUsing(ctor: src =>
                new ProcessBasedEmployee(
                    src.Name, src.InternalId, src.Department,
                    new Percent(src.Premium)))
            .ForAllMembers(memberOptions: opt => opt.Ignore());

        CreateMap<RateEmployeeCreateViewModel, RateBasedEmployee>()
            .ConstructUsing(ctor: src =>
                new RateBasedEmployee(
                    src.Name, src.InternalId,
                    new Money(src.Rate), src.Department,
                    (int)src.Premium))
            .ForAllMembers(memberOptions: opt => opt.Ignore());

        CreateMap<TechnologistCreateViewModel, Technologist>()
            .ConstructUsing(ctor: src =>
                new Technologist(
                    src.Name, src.InternalId,
                    new Percent(src.SalaryPercentage), src.Department))
            .ForAllMembers(memberOptions: opt => opt.Ignore());

        /* ---------- UPDATE MAPPINGS ---------- */

        // flat Update → base Employee
        CreateMap<EmployeeUpdateViewModel, Employee>(MemberList.None)
            .ForMember(destinationMember: d => d.InternalId, memberOptions: o => o.MapFrom(mapExpression: s => s.InternalId))
            .ForMember(destinationMember: d => d.Department, memberOptions: o => o.MapFrom(mapExpression: s => s.Department));

        // Process update → sets Premium
        CreateMap<ProcessEmployeeUpdateViewModel, ProcessBasedEmployee>(MemberList.None)
            .IncludeBase<EmployeeUpdateViewModel, Employee>()
            .ForMember(destinationMember: d => d.Premium,
                memberOptions: o => o.MapFrom(mapExpression: s => new Percent(s.Premium)));

        // Rate update → sets Rate
        CreateMap<RateEmployeeUpdateViewModel, RateBasedEmployee>(MemberList.None)
            .IncludeBase<ProcessEmployeeUpdateViewModel, ProcessBasedEmployee>()
            .ForMember(destinationMember: d => d.Rate,
                memberOptions: o => o.MapFrom(mapExpression: s => new Money(s.Rate)));

        // Technologist update → sets SalaryPercentage
        CreateMap<TechnologistUpdateViewModel, Technologist>(MemberList.None)
            .IncludeBase<EmployeeUpdateViewModel, Employee>()
            .ForMember(destinationMember: d => d.SalaryPercentage,
                memberOptions: o => o.MapFrom(mapExpression: s => new Percent(s.SalaryPercentage)));
    }
}