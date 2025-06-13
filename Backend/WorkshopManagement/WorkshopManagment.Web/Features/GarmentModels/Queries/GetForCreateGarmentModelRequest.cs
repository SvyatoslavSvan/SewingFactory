using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;

public record GetForCreateGarmentModelRequest(ClaimsPrincipal User) : IRequest<OperationResult<GetForCreateGarmentModelViewModel>>;

public class GetForCreateGarmentModelRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetForCreateGarmentModelRequest, OperationResult<GetForCreateGarmentModelViewModel>>
{
    public async Task<OperationResult<GetForCreateGarmentModelViewModel>> Handle(GetForCreateGarmentModelRequest request, CancellationToken cancellationToken) => OperationResult.CreateResult(
        new GetForCreateGarmentModelViewModel
        {
            Processes = mapper.Map<List<ReadProcessViewModel>>(await unitOfWork.GetRepository<Process>().GetAllAsync(TrackingType.NoTracking)),
            GarmentCategories = mapper.Map<List<ReadGarmentCategoryViewModel>>(await unitOfWork.GetRepository<GarmentCategory>().GetAllAsync(TrackingType.NoTracking))
        });
}