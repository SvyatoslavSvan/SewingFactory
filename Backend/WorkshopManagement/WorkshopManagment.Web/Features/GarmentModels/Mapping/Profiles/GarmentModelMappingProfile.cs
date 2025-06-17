using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Mapping.Profiles;

public sealed class GarmentModelMappingProfile : Profile
{
    public GarmentModelMappingProfile()
    {
        CreateMap<Money, decimal>()
            .ConvertUsing(mappingExpression: src => src.Amount);
        
        CreateMap<CreateGarmentModelViewModel, GarmentModel>()
            .ConstructUsing((src, ctx) =>
            {
                var loadedProcesses = ctx.Items["Processes"] as List<Process>
                                      ?? throw new SewingFactoryInvalidOperationException("Processes not passed in mapping context");
                var loadedCategory  = ctx.Items["Category"] as GarmentCategory
                                      ?? throw new SewingFactoryInvalidOperationException("Category not passed in mapping context");

                return new GarmentModel(
                    src.Name,
                    src.Description,
                    loadedProcesses,
                    loadedCategory,
                    new Money(src.Price));
            })
            .ForAllOtherMembers(opt => opt.Ignore());

        CreateMap<GarmentModel, ReadGarmentModelViewModel>().ConstructUsing(ctor: x => new ReadGarmentModelViewModel { Id = x.Id, Image = x.Image, Name = x.Name });
        CreateMap<UpdateGarmentModelViewModel, GarmentModel>()
            .ForMember(destinationMember: dest => dest.Name, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Name))
            .ForMember(destinationMember: dest => dest.Description, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Description))
            .ForMember(destinationMember: dest => dest.Image, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Image))
            .ForMember(destinationMember: dest => dest.Price, memberOptions: opt => opt.MapFrom(mapExpression: src => new Money(src.Price)))
            .ForMember(destinationMember: dest => dest.Category, memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.Processes, memberOptions: opt => opt.Ignore());

        CreateMap<GarmentModel, DetailsReadGarmentModelViewModel>()
            .ConstructUsing(ctor: garmentModel => new DetailsReadGarmentModelViewModel
            {
                Id = garmentModel.Id,
                Image = garmentModel.Image,
                Name = garmentModel.Name,
                Price = garmentModel.Price.Amount,
                Category = new ReadGarmentCategoryViewModel { Id = garmentModel.Category.Id, Name = garmentModel.Category.Name },
                Processes = garmentModel.Processes
                    .Select(process => new ReadProcessViewModel
                    {
                        Id = process.Id, DepartmentViewModel = new ReadDepartmentViewModel { Id = process.Id, Name = process.Name }, Name = process.Name, Price = process.Price.Amount
                    }).ToList()
            });
    }
}