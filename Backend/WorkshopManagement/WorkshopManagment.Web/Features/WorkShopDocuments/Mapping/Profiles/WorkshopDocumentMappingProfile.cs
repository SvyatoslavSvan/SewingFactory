using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Repeats;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Tasks;
using SewingFactory.Common.Domain.Base;
using System.Reflection;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Mapping.Profiles;

public class WorkshopDocumentMappingProfile : Profile
{
    public WorkshopDocumentMappingProfile()
    {
        CreateMap<EmployeeTaskRepeat, ReadEmployeeTaskRepeatViewModel>()
            .ForMember(destinationMember: x => x.Id,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Id))
            .ForMember(destinationMember: x => x.Repeats,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Repeats))
            .ForMember(destinationMember: dest => dest.Employees,
                memberOptions: opt => opt.MapFrom(mappingFunction: (
                        src,
                        dest,
                        destMember,
                        ctx) =>
                    ctx.Mapper.Map<EmployeeReadViewModel>(src.WorkShopEmployee)
                ));

        CreateMap<WorkshopTask, ReadWorkshopTaskViewModel>()
            .ForMember(destinationMember: dest => dest.EmployeeRepeats,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.EmployeeTaskRepeats))
            .ForMember(destinationMember: x => x.Id,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Id))
            .ForMember(destinationMember: dest => dest.EmployeeRepeats,
                memberOptions: opt => opt.MapFrom(mappingFunction: (
                        src,
                        dest,
                        destMember,
                        ctx) =>
                    ctx.Mapper.Map<List<ReadEmployeeTaskRepeatViewModel>>(src.EmployeeTaskRepeats)
                ))
            .ForMember(destinationMember: dest => dest.Process,
                memberOptions: opt => opt.MapFrom(mappingFunction: (
                        src,
                        dest,
                        destMember,
                        ctx) =>
                    ctx.Mapper.Map<ReadProcessViewModel>(src.Process)
                ));

        ;

        CreateMap<WorkshopDocument, DetailsReadWorkshopDocumentViewModel>()
            .ForMember(destinationMember: dest => dest.Id,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Id))
            .ForMember(destinationMember: dest => dest.Name,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Name))
            .ForMember(destinationMember: dest => dest.Date,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Date))
            .ForMember(destinationMember: dest => dest.CountOfModelsInvolved,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.CountOfModelsInvolved))
            .ForMember(destinationMember: dest => dest.WorkshopTasks,
                memberOptions: opt => opt.MapFrom(mappingFunction: (
                        src,
                        dest,
                        destMember,
                        ctx) =>
                    ctx.Mapper.Map<List<ReadWorkshopTaskViewModel>>(src.Tasks)
                ));

        CreateMap<WorkshopDocument, ReadWorkshopDocumentViewModel>()
            .ForMember(destinationMember: dest => dest.Name,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Name))
            .ForMember(destinationMember: x => x.Date,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Date))
            .ForMember(destinationMember: x => x.CountOfModelsInvolved,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.CountOfModelsInvolved))
            .ForMember(destinationMember: x => x.Id,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Id));

        CreateMap<UpdateWorkshopDocumentViewModel, WorkshopDocument>()
            .ForMember(destinationMember: x => x.Date,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Date))
            .ForMember(destinationMember: x => x.Name,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Name))
            .ForMember(destinationMember: x => x.CountOfModelsInvolved,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.CountOfModelsInvolved))
            .ForMember(destinationMember: x => x.Employees, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.GarmentModel, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.Tasks, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.Department, memberOptions: o => o.Ignore());

        ;
        CreateMap<UpdateEmployeeTaskRepeatViewModel, EmployeeTaskRepeat>()
            .ConstructUsing(ctor: (src, ctx) =>
            {
                var dict = (Dictionary<Guid, Employee>)ctx.Items["EmployeesById"];
                if (!dict.TryGetValue(src.EmployeeId, out var emp))
                {
                    throw new KeyNotFoundException($"Employee {src.EmployeeId} not loaded");
                }

                var repeat = new EmployeeTaskRepeat(emp, src.RepeatsCount);

                if (src.Id != Guid.Empty)
                {
                    typeof(Identity)
                        .GetField("<Id>k__BackingField",
                            BindingFlags.Instance | BindingFlags.NonPublic)!
                        .SetValue(repeat, src.Id);
                }

                return repeat;
            }).ForAllOtherMembers(memberOptions: x => x.Ignore());

        ;
    }
}