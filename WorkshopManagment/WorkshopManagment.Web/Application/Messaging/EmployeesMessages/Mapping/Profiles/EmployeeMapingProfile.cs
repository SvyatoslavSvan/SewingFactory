using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Mapping.Converters;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Mapping.Profiles;

public sealed class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {

        CreateMap<Guid,ProcessBasedEmployee>().ConvertUsing<EmployeeStubConverter>();

        CreateMap<Employee, EmployeeReadViewModel>()
            .ForMember(d => d.DepartmentViewModel,
                       o => o.MapFrom(s => new ReadDepartmentViewModel
                       {
                           Id = s.Department.Id,
                           Name = s.Department.Name
                       }));

        CreateMap<ProcessBasedEmployee, ProcessEmployeeReadViewModel>()
            .IncludeBase<Employee, EmployeeReadViewModel>()
            .ForMember(d => d.Premium, o => o.MapFrom(s => s.Premium.Value));

        CreateMap<RateBasedEmployee, RateEmployeeReadViewModel>()
            .IncludeBase<ProcessBasedEmployee, ProcessEmployeeReadViewModel>()
            .ForMember(d => d.Rate, o => o.MapFrom(s => s.Rate.Amount));

        CreateMap<Technologist, TechnologistReadViewModel>()
            .IncludeBase<Employee, EmployeeReadViewModel>()
            .ForMember(d => d.SalaryPercentage,
                       o => o.MapFrom(s => s.SalaryPercentage.Value));

        CreateMap<EmployeeCreateViewModel, Employee>()
            .ConstructUsing((src, ctx) => src switch
            {
                RateEmployeeCreateViewModel r => ctx.Mapper.Map<RateBasedEmployee>(r),
                TechnologistCreateViewModel t => ctx.Mapper.Map<Technologist>(t),
                ProcessEmployeeCreateViewModel p => ctx.Mapper.Map<ProcessBasedEmployee>(p),
                _ => throw new SewingFactoryArgumentOutOfRangeException(
                         nameof(src),
                         $"Unknown create DTO type: {src.GetType().Name}")
            })
            .ForMember(d => d.Department, o => o.Ignore());

        CreateMap<ProcessEmployeeCreateViewModel, ProcessBasedEmployee>()
            .ConstructUsing((src, ctx) =>
                new ProcessBasedEmployee(
                    src.Name,
                    src.InternalId,
                    ctx.Mapper.Map<Department>(src.DepartmentId),  
                    new Percent(src.Premium)))
            .ForAllMembers(o => o.Ignore());

        CreateMap<RateEmployeeCreateViewModel, RateBasedEmployee>()
            .ConstructUsing((src, ctx) =>
                new RateBasedEmployee(
                    src.Name,
                    src.InternalId,
                    new Money(src.Rate),
                    ctx.Mapper.Map<Department>(src.DepartmentId),
                    (int)src.Premium))
            .ForAllMembers(o => o.Ignore());

        CreateMap<TechnologistCreateViewModel, Technologist>()
            .ConstructUsing((src, ctx) =>
                new Technologist(
                    src.Name,
                    src.InternalId,
                    new Percent(src.SalaryPercentage),
                    ctx.Mapper.Map<Department>(src.DepartmentId)))
            .ForAllMembers(o => o.Ignore());

        CreateMap<EmployeeUpdateViewModel, Employee>()
            .ForMember(d => d.InternalId, o => o.MapFrom(s => s.InternalId))
            .ForMember(d => d.Department, o => o.Ignore())
            .ForAllOtherMembers(o => o.Ignore());

        CreateMap<ProcessEmployeeUpdateViewModel, ProcessBasedEmployee>()
            .IncludeBase<EmployeeUpdateViewModel, Employee>()
            .ForMember(d => d.Premium, o => o.MapFrom(s => new Percent(s.Premium)))
            .ForMember(d => d.Documents, o => o.Ignore())
            .ForAllOtherMembers(o => o.Ignore());

        CreateMap<RateEmployeeUpdateViewModel, RateBasedEmployee>()
            .IncludeBase<ProcessEmployeeUpdateViewModel, ProcessBasedEmployee>()
            .ForMember(d => d.Rate, o => o.MapFrom(s => new Money(s.Rate)))
            .ForMember(d => d.Documents, o => o.Ignore())
            .ForMember(d => d.Timesheets, o => o.Ignore())
            .ForAllOtherMembers(o => o.Ignore());

        CreateMap<TechnologistUpdateViewModel, Technologist>()
            .IncludeBase<EmployeeUpdateViewModel, Employee>()
            .ForMember(d => d.SalaryPercentage,
                       o => o.MapFrom(s => new Percent(s.SalaryPercentage)))
            .ForAllOtherMembers(o => o.Ignore());
    }
}
