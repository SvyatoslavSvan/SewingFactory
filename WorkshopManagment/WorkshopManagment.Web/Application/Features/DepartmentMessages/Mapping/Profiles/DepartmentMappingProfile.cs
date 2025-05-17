using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.Mapping.Profiles;

public sealed class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Guid, Department>()
            .ConvertUsing<DepartmentStubConverter>();

        CreateMap<Department, ReadDepartmentViewModel>();

        CreateMap<CreateDepartmentViewModel, Department>()
            .ConstructUsing(ctor: src => new Department(src.Name))
            .ForAllMembers(memberOptions: opt => opt.Ignore());

        CreateMap<UpdateDepartmentViewModel, Department>()
            .ForMember(destinationMember: d => d.Name, memberOptions: o => o.MapFrom(mapExpression: s => s.Name))
            .ForMember(destinationMember: d => d.Documents, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: d => d.Employees, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: d => d.Processes, memberOptions: o => o.Ignore());
    }
}