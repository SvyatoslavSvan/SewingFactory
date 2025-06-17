using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Publisher;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;

public sealed record CreateGarmentModelRequest(CreateGarmentModelViewModel Model, ClaimsPrincipal User)
    : CreateRequest<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel>(Model, User);

public sealed class CreateGarmentModelHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper, IGarmentModelPublisher publisher)
    : CreateRequestHandler<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel>(unitOfWork, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<DetailsReadGarmentModelViewModel>> Handle(
        CreateRequest<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel> request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<DetailsReadGarmentModelViewModel>();
        var processes = await LoadProcesses(request);
        var garmentCategory = await LoadGarmentCategory(request);
        var garmentModel = await CreateGarmentModel(request, processes, garmentCategory, cancellationToken);
        return await EnsureCreated(garmentModel, operation);
    }

    private async Task<IList<Process>> LoadProcesses(CreateRequest<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel> request)
    {
        var processes = await unitOfWork.GetRepository<Process>().GetAllAsync(
            predicate: x => request.Model.ProcessesIds.Contains(x.Id),
            trackingType: TrackingType.Tracking);

        return processes;
    }

    private async Task<GarmentCategory> LoadGarmentCategory(CreateRequest<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel> request)
    {
        var garmentCategory = await unitOfWork.GetRepository<GarmentCategory>().GetFirstOrDefaultAsync(predicate: category => category.Id == request.Model.GarmentCategoryId,
            trackingType: TrackingType.Tracking);

        if (garmentCategory is null)
        {
            throw new SewingFactoryNotFoundException($"{nameof(GarmentCategory)} with key {request.Model.GarmentCategoryId} not found.");
        }

        return garmentCategory;
    }

    private async Task<OperationResult<DetailsReadGarmentModelViewModel>> EnsureCreated(GarmentModel garmentModel, OperationResult<DetailsReadGarmentModelViewModel> operation)
    {
        if (unitOfWork.Result.Ok)
        {
            return await PublishAndMap(garmentModel, operation);
        }

        operation.AddError($"An error occurred while creating {nameof(GarmentModel)} with Id {garmentModel.Id}.");

        return operation;
    }

    private async Task<OperationResult<DetailsReadGarmentModelViewModel>> PublishAndMap(GarmentModel garmentModel, OperationResult<DetailsReadGarmentModelViewModel> operation)
    {
        await publisher.PublishCreatedAsync(garmentModel);
        operation.Result = _mapper.Map<DetailsReadGarmentModelViewModel>(garmentModel);

        return operation;
    }

    private async Task<GarmentModel> CreateGarmentModel(
        CreateRequest<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel> request,
        IList<Process> processes,
        GarmentCategory category,
        CancellationToken cancellationToken)
    {
        var garmentModel = _mapper.Map<GarmentModel>(
            request.Model,
            opts: opts =>
            {
                opts.Items["Processes"] = processes;
                opts.Items["Category"] = category;
            });

        await unitOfWork.GetRepository<GarmentModel>().InsertAsync(garmentModel, cancellationToken);
        await unitOfWork.SaveChangesAsync();

        return garmentModel;
    }
}