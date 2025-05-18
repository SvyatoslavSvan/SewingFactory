using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Mapping.Converters;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Mapping.Profiles;

public sealed class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Guid, ProcessBasedEmployee>().ConvertUsing<EmployeeStubConverter>();

        CreateMap<Employee, EmployeeReadViewModel>()
            .ForMember(destinationMember: d => d.DepartmentViewModel,
                memberOptions: o => o.MapFrom(mapExpression: s => new ReadDepartmentViewModel { Id = s.Department.Id, Name = s.Department.Name }));

        CreateMap<ProcessBasedEmployee, ProcessEmployeeReadViewModel>()
            .IncludeBase<Employee, EmployeeReadViewModel>()
            .ForMember(destinationMember: d => d.Premium, memberOptions: o => o.MapFrom(mapExpression: s => s.Premium.Value));

        CreateMap<RateBasedEmployee, RateEmployeeReadViewModel>()
            .IncludeBase<Employee, EmployeeReadViewModel>()
            .ForMember(destinationMember: d => d.Rate, memberOptions: o => o.MapFrom(mapExpression: s => s.Rate.Amount))
            .ForMember(destinationMember: d => d.Premium, memberOptions: o => o.MapFrom(mapExpression: s => s.Premium.Value)); ;

        CreateMap<Technologist, TechnologistReadViewModel>()
            .IncludeBase<Employee, EmployeeReadViewModel>()
            .ForMember(destinationMember: d => d.SalaryPercentage,
                memberOptions: o => o.MapFrom(mapExpression: s => s.SalaryPercentage.Value));

        CreateMap<EmployeeCreateViewModel, Employee>()
            .ConstructUsing(ctor: (src, ctx) => src switch
            {
                RateEmployeeCreateViewModel r => ctx.Mapper.Map<RateBasedEmployee>(r),
                TechnologistCreateViewModel t => ctx.Mapper.Map<Technologist>(t),
                ProcessEmployeeCreateViewModel p => ctx.Mapper.Map<ProcessBasedEmployee>(p),
                _ => throw new SewingFactoryArgumentOutOfRangeException(
                    nameof(src),
                    $"Unknown create DTO type: {src.GetType().Name}")
            })
            .ForMember(destinationMember: d => d.Department, memberOptions: o => o.Ignore())
            .ForMember(x => x.Documents, o => o.Ignore());

        CreateMap<ProcessEmployeeCreateViewModel, ProcessBasedEmployee>()
            .ConstructUsing(ctor: (src, ctx) =>
                new ProcessBasedEmployee(
                    src.Name,
                    src.InternalId,
                    ctx.Mapper.Map<Department>(src.DepartmentId),
                    new Percent(src.Premium)))
            .ForAllMembers(memberOptions: o => o.Ignore());

        CreateMap<RateEmployeeCreateViewModel, RateBasedEmployee>()
            .ConstructUsing(ctor: (src, ctx) =>
                new RateBasedEmployee(
                    src.Name,
                    src.InternalId,
                    new Money(src.Rate),
                    ctx.Mapper.Map<Department>(src.DepartmentId),
                    (int)src.Premium))
            .ForAllMembers(memberOptions: o => o.Ignore());

        CreateMap<TechnologistCreateViewModel, Technologist>()
            .ConstructUsing(ctor: (src, ctx) =>
                new Technologist(
                    src.Name,
                    src.InternalId,
                    new Percent(src.SalaryPercentage),
                    ctx.Mapper.Map<Department>(src.DepartmentId)))
            .ForAllMembers(memberOptions: o => o.Ignore());

        CreateMap<EmployeeUpdateViewModel, Employee>()
            .ForMember(destinationMember: d => d.InternalId, memberOptions: o => o.MapFrom(mapExpression: s => s.InternalId))
            .ForMember(destinationMember: d => d.Department, memberOptions: o => o.Ignore())
            .ForAllOtherMembers(memberOptions: o => o.Ignore());

        CreateMap<ProcessEmployeeUpdateViewModel, ProcessBasedEmployee>()
            .IncludeBase<EmployeeUpdateViewModel, Employee>()
            .ForMember(destinationMember: d => d.Premium, memberOptions: o => o.MapFrom(mapExpression: s => new Percent(s.Premium)))
            .ForMember(destinationMember: d => d.Documents, memberOptions: o => o.Ignore())
            .ForAllOtherMembers(memberOptions: o => o.Ignore());

        CreateMap<RateEmployeeUpdateViewModel, RateBasedEmployee>()
            .IncludeBase<EmployeeUpdateViewModel, Employee>()
            .ForMember(destinationMember: d => d.Rate, memberOptions: o => o.MapFrom(mapExpression: s => new Money(s.Rate)))
            .ForMember(destinationMember: d => d.Documents, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: d => d.Timesheets, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: d => d.Premium, memberOptions: o => o.MapFrom(mapExpression: s => new Percent(s.Premium)))
            .ForMember(destinationMember: d => d.Documents, memberOptions: o => o.Ignore())
            .ForAllOtherMembers(memberOptions: o => o.Ignore());

        CreateMap<TechnologistUpdateViewModel, Technologist>()
            .IncludeBase<EmployeeUpdateViewModel, Employee>()
            .ForMember(destinationMember: d => d.SalaryPercentage,
                memberOptions: o => o.MapFrom(mapExpression: s => new Percent(s.SalaryPercentage)))
            .ForAllOtherMembers(memberOptions: o => o.Ignore());
    }
}