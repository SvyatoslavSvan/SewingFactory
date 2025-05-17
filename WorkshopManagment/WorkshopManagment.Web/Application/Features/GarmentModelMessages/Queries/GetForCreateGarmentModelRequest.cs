using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.Queries;

public record GetForCreateGarmentModelRequest(ClaimsPrincipal User) : IRequest<OperationResult<GetForCreateGarmentModelViewModel>>;

public class GetForCreateGarmentModelRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetForCreateGarmentModelRequest, OperationResult<GetForCreateGarmentModelViewModel>>
{
    public async Task<OperationResult<GetForCreateGarmentModelViewModel>> Handle(GetForCreateGarmentModelRequest request, CancellationToken cancellationToken) => OperationResult.CreateResult(
        new GetForCreateGarmentModelViewModel
        {
            Processes = mapper.Map<List<ReadProcessViewModel>>(await unitOfWork.GetRepository<Process>().GetAllAsync(true)),
            GarmentCategories = mapper.Map<List<ReadGarmentCategoryViewModel>>(await unitOfWork.GetRepository<GarmentCategory>().GetAllAsync(true))
        });
}