using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.Mapping.Converters;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.Mapping.Profiles;

public sealed class GarmentModelMappingProfile : Profile
{
    public GarmentModelMappingProfile()
    {
        CreateMap<Guid, GarmentModel>().ConvertUsing<GarmentModelStubConverter>();

        CreateMap<CreateGarmentModelViewModel, GarmentModel>().ConvertUsing<CreateGarmentModelConverter>();

        CreateMap<GarmentModel, ReadGarmentModelViewModel>().ConstructUsing(ctor: x => new ReadGarmentModelViewModel { Id = x.Id, Image = x.Image, Name = x.Name });
        CreateMap<UpdateGarmentModelViewModel, GarmentModel>()
            .ForMember(destinationMember: dest => dest.Name, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Name))
            .ForMember(destinationMember: dest => dest.Description, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Description))
            .ForMember(destinationMember: dest => dest.Image, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Image))
            .ForMember(destinationMember: dest => dest.Category, memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.Processes, memberOptions: opt => opt.Ignore());

        CreateMap<GarmentModel, DetailsReadGarmentModelViewModel>()
            .ConstructUsing(ctor: garmentModel => new DetailsReadGarmentModelViewModel
            {
                Id = garmentModel.Id,
                Image = garmentModel.Image,
                Name = garmentModel.Name,
                Category = new ReadGarmentCategoryViewModel { Id = garmentModel.Category.Id, Name = garmentModel.Category.Name },
                Processes = garmentModel.Processes
                    .Select(process => new ReadProcessViewModel { Id = process.Id, DepartmentViewModel = new ReadDepartmentViewModel {Id = process.Id, Name = process.Name}, Name = process.Name, Price = process.Price.Amount }).ToList()
            });
    }
}