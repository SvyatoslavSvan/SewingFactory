using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;

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
            include: queryable => queryable.Include(navigationPropertyPath: garmentModel => garmentModel.Processes),
            trackingType: TrackingType.Tracking);

        if (entity == null)
        {
            operation.AddError(new SewingFactoryNotFoundException($"GarmentModel {request.Model.Id} not found"));

            return operation;
        }

        _mapper.Map(request.Model, entity);
        var processes = _mapper.Map<List<Process>>(request.Model.ProcessesIds);
        entity.ReplaceProcesses(processes);
        entity.Category = _mapper.Map<GarmentCategory>(request.Model.GarmentCategoryId);
        ;
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