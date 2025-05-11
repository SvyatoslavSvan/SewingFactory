using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Mapping.Profiles;

public sealed class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Guid, Department>()
            .ConvertUsing<DepartmentStubConverter>();

        CreateMap<Department, ReadDepartmentViewModel>();

        CreateMap<CreateDepartmentViewModel, Department>()
            .ConstructUsing(src => new Department(src.Name))
            .ForAllMembers(opt => opt.Ignore());   

        CreateMap<UpdateDepartmentViewModel, Department>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Documents, o => o.Ignore())
            .ForMember(d => d.Employees, o => o.Ignore())
            .ForMember(d => d.Processes, o => o.Ignore());
    }
}