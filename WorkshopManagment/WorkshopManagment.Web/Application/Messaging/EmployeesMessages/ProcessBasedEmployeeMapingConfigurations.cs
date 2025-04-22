using AutoMapper;
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
    }
}