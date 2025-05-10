using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages;

public sealed class ProcessProfile : Profile
{
    public ProcessProfile()
    {
        CreateMap<CreateProcessViewModel, Process>()
            .ConstructUsing(ctor: src =>
                new Process(
                    src.Name, src.Department,
                    new Money(src.Price)
                )
            );

        CreateMap<UpdateProcessViewModel, Process>()
            .ForMember(destinationMember: dest => dest.Name,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Name))
            .ForMember(destinationMember: dest => dest.Price,
                memberOptions: opt => opt.MapFrom(mapExpression: src => new Money(src.Price)))
            .ForAllMembers(memberOptions: opt => opt.Ignore());

        CreateMap<Process, ReadProcessViewModel>()
            .ForMember(
                destinationMember: dest => dest.Price,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Price.Amount)
            );
    }
}