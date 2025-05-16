using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Repeats;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Tasks;
using SewingFactory.Common.Domain.Base;
using System.Reflection;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.Mapping.Profiles
{
    public class WorkshopDocumentMappingProfile : Profile
    {
        public WorkshopDocumentMappingProfile()
        {
            CreateMap<EmployeeTaskRepeat, ReadEmployeeTaskRepeatViewModel>()
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Repeats,
                    opt => opt.MapFrom(x => x.Repeats))
                .ForMember(dest => dest.Employees,
                    opt => opt.MapFrom((
                            src,
                            dest,
                            destMember,
                            ctx) =>
                        ctx.Mapper.Map<EmployeeReadViewModel>(src.WorkShopEmployee)
                    ));

            CreateMap<WorkshopTask, ReadWorkshopTaskViewModel>()
                .ForMember(dest => dest.EmployeeRepeats,
                    opt => opt.MapFrom(src => src.EmployeeTaskRepeats))
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.EmployeeRepeats,
                    opt => opt.MapFrom((
                            src,
                            dest,
                            destMember,
                            ctx) =>
                        ctx.Mapper.Map<List<ReadEmployeeTaskRepeatViewModel>>(src.EmployeeTaskRepeats)
                    ))
                .ForMember(dest => dest.Process,
                    opt => opt.MapFrom((
                            src,
                            dest,
                            destMember,
                            ctx) =>
                        ctx.Mapper.Map<ReadProcessViewModel>(src.Process)
                    ));

            ;

            CreateMap<WorkshopDocument, DetailsReadWorkshopDocumentViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.CountOfModelsInvolved,
                    opt => opt.MapFrom(src => src.CountOfModelsInvolved))
                .ForMember(dest => dest.WorkshopTasks,
                    opt => opt.MapFrom((
                            src,
                            dest,
                            destMember,
                            ctx) =>
                        ctx.Mapper.Map<List<ReadWorkshopTaskViewModel>>(src.Tasks)
                    ));

            CreateMap<WorkshopDocument, ReadWorkshopDocumentViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.Date,
                    opt => opt.MapFrom(x => x.Date))
                .ForMember(x => x.CountOfModelsInvolved,
                    opt => opt.MapFrom(x => x.CountOfModelsInvolved))
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(x => x.Id));

            CreateMap<UpdateWorkshopDocumentViewModel, WorkshopDocument>()
                .ForMember(x => x.Date,
                    opt => opt.MapFrom(x => x.Date))
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Name,
                    opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.CountOfModelsInvolved,
                    opt => opt.MapFrom(x => x.CountOfModelsInvolved))
                .ForMember(dest => dest.GarmentModel,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Department,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Tasks,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Employees,
                    opt => opt.Ignore());

            ;

            CreateMap<UpdateWorkShopTaskViewModel, WorkshopTask>()
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.EmployeeTaskRepeats,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Process,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Document,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EmployeesInvolved,
                    opt => opt.Ignore())
                .AfterMap((src, dest, ctx) =>
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
                }); ;

            ;

            CreateMap<UpdateEmployeeTaskRepeatViewModel, EmployeeTaskRepeat>()
                .ForMember(x => x.Repeats,
                    opt => opt.MapFrom(x => x.RepeatsCount))
                .ForMember(x => x.WorkShopEmployee,
                    opt => opt.MapFrom((
                            src,
                            dest,
                            destMember,
                            ctx) =>
                        ctx.Mapper.Map<ProcessBasedEmployee>(src.EmployeeId)
                    ))
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.WorkshopTask,
                    opt => opt.Ignore());

            ;
        }
    }

}