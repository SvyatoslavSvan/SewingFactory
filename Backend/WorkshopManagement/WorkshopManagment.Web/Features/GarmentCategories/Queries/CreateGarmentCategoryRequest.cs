using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Publisher;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Queries;

public sealed record CreateGarmentCategoryRequest(CreateGarmentCategoryViewModel Model, ClaimsPrincipal User)
    : CreateRequest<CreateGarmentCategoryViewModel, GarmentCategory, ReadGarmentCategoryViewModel>(Model, User);

public sealed class CreateGarmentCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper, IGarmentCategoryPublisher publisher)
    : CreateRequestHandler<CreateGarmentCategoryViewModel, GarmentCategory, ReadGarmentCategoryViewModel>(unitOfWork, mapper)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<OperationResult<ReadGarmentCategoryViewModel>> Handle(
        CreateRequest<CreateGarmentCategoryViewModel, GarmentCategory, ReadGarmentCategoryViewModel> request, CancellationToken cancellationToken)
    {
        var category = new GarmentCategory(request.Model.Name, Guid.NewGuid());
        await InsertAndPublish(cancellationToken, category);

        return EnsureCreated(category);
    }

    private async Task InsertAndPublish(CancellationToken cancellationToken, GarmentCategory category)
    {
        await _unitOfWork.GetRepository<GarmentCategory>().InsertAsync(category, cancellationToken);
        await publisher.PublishCreatedAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }
}