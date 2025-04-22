using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;

public record CreateRequest<TCreateViewModel,TEntity, TReadViewModel>(TCreateViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<TReadViewModel>> 
{
};

public class CreateRequestHandler<TCreateViewModel,TEntity, TReadViewModel>(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateRequest<TCreateViewModel,TEntity, TReadViewModel>, OperationResult<TReadViewModel>> where TEntity : Identity
{
    public async Task<OperationResult<TReadViewModel>> Handle(CreateRequest<TCreateViewModel,TEntity, TReadViewModel> request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<TReadViewModel>();
        var employee = mapper.Map<TEntity>(request.Model);
        await unitOfWork.GetRepository<TEntity>().InsertAsync(employee, cancellationToken);
        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(unitOfWork.LastSaveChangesResult.Exception
                               ?? new SewingFactoryDatabaseSaveException($"Error while saving entity{nameof(ProcessBasedEmployee)}"));

            return operation;
        }

        operation.Result = mapper.Map<TReadViewModel>(employee);

        return operation;
    }
}