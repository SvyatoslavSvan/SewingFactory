using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.Mapping;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.Queries;

public sealed record UpdateGarmentModelRequest(UpdateGarmentModelViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<UpdateGarmentModelViewModel, GarmentModel>(Model, User);

public sealed class UpdateGarmentModelHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper) : UpdateRequestHandler<UpdateGarmentModelViewModel, GarmentModel>(unitOfWork, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<UpdateGarmentModelViewModel>> Handle(UpdateRequest<UpdateGarmentModelViewModel, GarmentModel> request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<UpdateGarmentModelViewModel>();
        var entity = await unitOfWork.GetRepository<GarmentModel>().GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Model.Id,
            trackingType: TrackingType.Tracking);

        if (entity == null)
        {
            operation.AddError(new SewingFactoryNotFoundException($"GarmentModel {request.Model.Id} not found"));

            return operation;
        }

        _mapper.Map(request.Model, entity);
        entity.ReplaceProcesses(GarmentModelStubHelper.GetProcessStubs(request.Model, unitOfWork));
        entity.Category = GarmentModelStubHelper.GetGarmentCategoryStub(request.Model, unitOfWork);
        unitOfWork.GetRepository<GarmentModel>().Update(entity);
        await unitOfWork.SaveChangesAsync();
        if (unitOfWork.Result.Ok)
        {
            operation.Result = request.Model;

            return operation;
        }

        operation.AddError($"An error occurred while updating {nameof(GarmentModel)} with Id {entity.Id}.");

        return operation;
    }
}