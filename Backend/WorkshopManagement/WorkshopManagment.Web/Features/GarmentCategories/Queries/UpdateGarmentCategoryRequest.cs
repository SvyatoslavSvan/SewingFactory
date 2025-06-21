using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Publisher;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Queries;

public sealed record UpdateGarmentCategoryRequest(UpdateGarmentCategoryViewModel Model, ClaimsPrincipal User) : UpdateRequest<UpdateGarmentCategoryViewModel, GarmentCategory>(Model, User);

public sealed class UpdateRequestGarmentCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper, IGarmentCategoryPublisher publisher)
    : UpdateRequestHandler<UpdateGarmentCategoryViewModel, GarmentCategory>(unitOfWork, mapper)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<UpdateGarmentCategoryViewModel>> Handle(UpdateRequest<UpdateGarmentCategoryViewModel, GarmentCategory> request, CancellationToken cancellationToken)
    {
        if (await LoadEntity(request) is not { } entity)
        {
            return AddEntityNotFoundError(request.Model.Id);
        }

        await UpdateAndPublish(request, entity);

        return EnsureUpdated(request);
    }

    private async Task UpdateAndPublish(UpdateRequest<UpdateGarmentCategoryViewModel, GarmentCategory> request, GarmentCategory entity)
    {
        _mapper.Map(request.Model, entity);
        await publisher.PublishUpdatedAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}