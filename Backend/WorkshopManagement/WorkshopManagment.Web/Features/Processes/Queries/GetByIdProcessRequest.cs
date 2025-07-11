﻿using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using OpenIddict.Abstractions;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Queries;

public sealed record GetByIdProcessRequest(ClaimsPrincipal User, Guid Id)
    : GetByIdRequest<Process, ReadProcessViewModel>(User, Id);

public sealed class GetByIdProcessHandler(ApplicationDbContext dbContext, IMapper mapper)
    : GetByIdRequestHandler<Process, ReadProcessViewModel>(dbContext, mapper)
{
    public override async Task<OperationResult<ReadProcessViewModel>> Handle(GetByIdRequest<Process, ReadProcessViewModel> request, CancellationToken cancellationToken)
    {
        var baseResult = await base.Handle(request, cancellationToken);
        MaskPriceIfNoFinanceAccess(request, baseResult);
        ;

        return baseResult;
    }

    private static void MaskPriceIfNoFinanceAccess(GetByIdRequest<Process, ReadProcessViewModel> request, OperationResult<ReadProcessViewModel> baseResult)
    {
        if (!request.User.HasClaim(AppData.FinanceAccess) && baseResult.Ok)
        {
            baseResult.Result!.Price = 0;
        }
    }
}