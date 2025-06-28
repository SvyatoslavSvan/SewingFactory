using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using OpenIddict.Abstractions;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Queries;

public sealed record GetAllProcessRequest(ClaimsPrincipal User)
    : GetAllRequest<Process, ReadProcessViewModel>(User);

public sealed class GetAllProcessHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetAllRequestHandler<Process, ReadProcessViewModel>(unitOfWork, mapper)
{
    public override async Task<OperationResult<IEnumerable<ReadProcessViewModel>>> Handle(GetAllRequest<Process, ReadProcessViewModel> request, CancellationToken cancellationToken)
    {
        var baseResult = await base.Handle(request, cancellationToken);
        MaskPricesIfNoFinanceAccess(request, baseResult);

        return baseResult;
    }

    private static void MaskPricesIfNoFinanceAccess(GetAllRequest<Process, ReadProcessViewModel> request, OperationResult<IEnumerable<ReadProcessViewModel>> baseResult)
    {
        if (!request.User.HasClaim(AppData.FinanceAccess) && baseResult.Ok)
        {
            SetDefaultProcessPrices(baseResult);
        }
    }

    private static void SetDefaultProcessPrices(OperationResult<IEnumerable<ReadProcessViewModel>> baseResult)
    {
        foreach (var viewModel in baseResult.Result!)
        {
            viewModel.Price = 0;
        }
    }
}