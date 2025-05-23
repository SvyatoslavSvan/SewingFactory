﻿using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.Mapping;

public sealed class ProcessProfile : Profile
{
    public ProcessProfile()
    {
        CreateMap<CreateProcessViewModel, Process>()
            .ConstructUsing(ctor: (
                src,
                ctx) => new Process(
                src.Name,
                ctx.Mapper.Map<Department>(src.DepartmentId),
                new Money(src.Price)
            ))
            .ForMember(destinationMember: dest => dest.Department,
                memberOptions: opt => opt.Ignore())
            .ForAllOtherMembers(memberOptions: opt => opt.Ignore());

        CreateMap<UpdateProcessViewModel, Process>()
            .ForMember(destinationMember: dest => dest.Name, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Name))
            .ForMember(destinationMember: dest => dest.Price, memberOptions: opt => opt.MapFrom(mapExpression: src => new Money(src.Price)))
            .ForMember(destinationMember: dest => dest.Department, memberOptions: opt => opt.Ignore())
            .ForAllOtherMembers(memberOptions: opt => opt.Ignore());

        CreateMap<Process, ReadProcessViewModel>()
            .ForMember(destinationMember: x => x.Name, memberOptions: opt => opt.MapFrom(mapExpression: x => x.Name))
            .ForMember(destinationMember: x => x.Id, memberOptions: opt => opt.MapFrom(mapExpression: x => x.Id))
            .ForMember(destinationMember: dest => dest.Price,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Price.Amount))
            .ForMember(destinationMember: dest => dest.DepartmentViewModel,
                memberOptions: opt => opt.MapFrom(mapExpression: src => new ReadDepartmentViewModel { Id = src.Department.Id, Name = src.Department.Name }))
            .ForAllOtherMembers(memberOptions: opt => opt.Ignore());
    }
}