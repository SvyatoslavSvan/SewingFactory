using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Publisher;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;

public sealed record UpdateGarmentModelRequest(UpdateGarmentModelViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<UpdateGarmentModelViewModel, GarmentModel>(Model, User);

public sealed class UpdateGarmentModelHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper, IGarmentModelPublisher publisher) : UpdateRequestHandler<UpdateGarmentModelViewModel, GarmentModel>(unitOfWork, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<UpdateGarmentModelViewModel>> Handle(UpdateRequest<UpdateGarmentModelViewModel, GarmentModel> request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<UpdateGarmentModelViewModel>();
        var garmentModel = await LoadGarmentModel(request);

        if (garmentModel is null)
        {
            return AddGarmentModelNotFoundError(request, operation);
        }
        
        await UpdateGarmentModel(request, garmentModel);
        if (unitOfWork.Result.Ok)
        {
            return await Publish(request, garmentModel, operation);
        }
        return AddDatabaseSaveError(operation, garmentModel);
    }

    private static OperationResult<UpdateGarmentModelViewModel> AddGarmentModelNotFoundError(UpdateRequest<UpdateGarmentModelViewModel, GarmentModel> request, OperationResult<UpdateGarmentModelViewModel> operation)
    {
        operation.AddError(new SewingFactoryNotFoundException($"GarmentModel {request.Model.Id} not found"));

        return operation;
    }

    private OperationResult<UpdateGarmentModelViewModel> AddDatabaseSaveError(OperationResult<UpdateGarmentModelViewModel> operation, GarmentModel garmentModel)
    {
        operation.AddError($"An error occurred while updating {nameof(GarmentModel)} with Id {garmentModel.Id}.",
            unitOfWork.Result.Exception ?? new Exception("Unknown error"));

        return operation;
    }

    private async Task UpdateGarmentModel(UpdateRequest<UpdateGarmentModelViewModel, GarmentModel> request, GarmentModel garmentModel)
    {
        _mapper.Map(request.Model, garmentModel);
        await UpdateCategory(request, garmentModel);
        await UpdateProcesses(request, garmentModel);
        
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<GarmentModel?> LoadGarmentModel(UpdateRequest<UpdateGarmentModelViewModel, GarmentModel> request) => await unitOfWork.GetRepository<GarmentModel>()
        .GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Model.Id,
            include: queryable => queryable.Include(navigationPropertyPath: garmentModel => garmentModel.Processes)
                .Include(garmentModel => garmentModel.Category),
            trackingType: TrackingType.Tracking);

    private async Task<OperationResult<UpdateGarmentModelViewModel>> Publish(UpdateRequest<UpdateGarmentModelViewModel, GarmentModel> request, GarmentModel garmentModel, OperationResult<UpdateGarmentModelViewModel> operation)
    {
        await publisher.PublishUpdatedAsync(garmentModel);
        operation.Result = request.Model;
        return operation;
    }

    private async Task UpdateProcesses(UpdateRequest<UpdateGarmentModelViewModel, GarmentModel> request, GarmentModel garmentModel)
    {
        if (!garmentModel.Processes
                .Select(p => p.Id)
                .ToHashSet()
                .SetEquals(request.Model.ProcessesIds))
        {
            garmentModel.ReplaceProcesses(await unitOfWork.GetRepository<Process>()
                .GetAllAsync(predicate: process => request.Model.ProcessesIds.Contains(process.Id),
                    trackingType: TrackingType.Tracking));
        }
    }

    private async Task UpdateCategory(UpdateRequest<UpdateGarmentModelViewModel, GarmentModel> request, GarmentModel garmentModel)
    {
        if (garmentModel.Category.Id != request.Model.GarmentCategoryId)
        {
            garmentModel.Category = await unitOfWork.GetRepository<GarmentCategory>()
                                        .GetFirstOrDefaultAsync(predicate: category => category.Id == request.Model.GarmentCategoryId,
                                            trackingType: TrackingType.Tracking) ??
                                    throw new SewingFactoryNotFoundException($"GarmentCategory {request.Model.GarmentCategoryId} not found");
        }
    }
}