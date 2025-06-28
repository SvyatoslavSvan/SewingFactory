using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using Calabonga.UnitOfWork;
using OpenIddict.Abstractions;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Queries;

public sealed record GetPagedProcessRequest(ClaimsPrincipal User, int PageIndex, int PageSize)
    : GetPagedRequest<Process, ReadProcessViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedProcessHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetPagedRequestHandler<Process, ReadProcessViewModel>(unitOfWork, mapper)
{
    public override async Task<OperationResult<IPagedList<ReadProcessViewModel>>> Handle(GetPagedRequest<Process, ReadProcessViewModel> request, CancellationToken cancellationToken)
    {
        var baseResult = await base.Handle(request, cancellationToken);
        MaskPricesIfNoFinanceAccess(request, baseResult);
        ;

        return baseResult;
    }

    private static void MaskPricesIfNoFinanceAccess(GetPagedRequest<Process, ReadProcessViewModel> request, OperationResult<IPagedList<ReadProcessViewModel>> baseResult)
    {
        if (!request.User.HasClaim(AppData.FinanceAccess) && baseResult.Ok)
        {
            SetDefaultProcessPrices(baseResult);
        }
    }

    private static void SetDefaultProcessPrices(OperationResult<IPagedList<ReadProcessViewModel>> baseResult)
    {
        foreach (var viewModel in baseResult.Result!.Items)
        {
            viewModel.Price = 0;
        }
    }
}