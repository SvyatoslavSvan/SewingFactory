using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Repeats;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Tasks;
using SewingFactory.Common.Domain.Base;
using System.Reflection;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.Mapping.Profiles;

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
            .ForMember(destinationMember: x => x.Id,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Id))
            .ForMember(destinationMember: x => x.Name,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Name))
            .ForMember(destinationMember: x => x.CountOfModelsInvolved,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.CountOfModelsInvolved))
            .ForMember(destinationMember: dest => dest.GarmentModel,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.Department,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.Tasks,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.Employees,
                memberOptions: opt => opt.Ignore());

        ;

        CreateMap<UpdateWorkShopTaskViewModel, WorkshopTask>()
            .ForMember(destinationMember: x => x.Id,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Id))
            .ForMember(destinationMember: x => x.EmployeeTaskRepeats,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.Process,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.Document,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.EmployeesInvolved,
                memberOptions: opt => opt.Ignore())
            .AfterMap(afterFunction: (src, dest, ctx) =>
            {
                foreach (var vm in src.EmployeeRepeats)
                {
                    dest.AddOrUpdateEmployeeRepeat(
                        ctx.Mapper.Map<EmployeeTaskRepeat>(vm)
                    );
                }

                typeof(Identity)
                    .GetField("<Id>k__BackingField",
                        BindingFlags.Instance | BindingFlags.NonPublic)!
                    .SetValue(dest, src.Id);
            });

        ;

        ;

        CreateMap<UpdateEmployeeTaskRepeatViewModel, EmployeeTaskRepeat>()
            .ForMember(destinationMember: x => x.Repeats,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.RepeatsCount))
            .ForMember(destinationMember: x => x.WorkShopEmployee,
                memberOptions: opt => opt.MapFrom(mappingFunction: (
                        src,
                        dest,
                        destMember,
                        ctx) =>
                    ctx.Mapper.Map<ProcessBasedEmployee>(src.EmployeeId)
                ))
            .ForMember(destinationMember: x => x.Id,
                memberOptions: opt => opt.MapFrom(mapExpression: x => x.Id))
            .ForMember(destinationMember: dest => dest.WorkshopTask,
                memberOptions: opt => opt.Ignore());

        ;
    }
}