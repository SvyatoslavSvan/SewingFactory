using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.Mapping;

public sealed class ProcessProfile : Profile
{
    public ProcessProfile()
    {
        CreateMap<CreateProcessViewModel, Process>()
            .ConstructUsing((src, ctx) =>
                new Process(
                    src.Name,
                    default!,
                    new Money(src.Price)))
            .ForMember(dest => dest.Department, opt => opt.Ignore());

        CreateMap<UpdateProcessViewModel, Process>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Money(src.Price)))
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForAllOtherMembers(opt => opt.Ignore());

        CreateMap<Process, ReadProcessViewModel>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(dest => dest.Price,
                opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.DepartmentViewModel,
                opt => opt.MapFrom(src => new ReadDepartmentViewModel
                {
                    Id = src.Department.Id,
                    Name = src.Department.Name
                }))
            .ForAllOtherMembers(opt => opt.Ignore());
    }
}