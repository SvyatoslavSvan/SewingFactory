using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

public sealed record GetPointOfSaleByIdRequest(
    ClaimsPrincipal User,
    Guid Id)
    : GetByIdRequest<PointOfSale, PointOfSaleDetailsReadViewModel>(User,
        Id);

public sealed class GetPointOfSaleByIdHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper)
    : GetByIdRequestHandler<PointOfSale, PointOfSaleDetailsReadViewModel>(unitOfWork,
        mapper)
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public override async Task<Operation<PointOfSaleDetailsReadViewModel, SewingFactoryNotFoundException>> Handle(
        GetByIdRequest<PointOfSale, PointOfSaleDetailsReadViewModel> request,
        CancellationToken cancellationToken)
    {
        var pointOfSale = await _unitOfWork.GetRepository<PointOfSale>()
            .GetFirstOrDefaultAsync(predicate: pointOfSale => pointOfSale.Id == request.Id,
                include: queryable => queryable.Include(navigationPropertyPath: pointOfSale => pointOfSale.StockItems)
                    .ThenInclude(navigationPropertyPath: stockItem => stockItem.GarmentModel)
                    .Include(navigationPropertyPath: pointOfSale => pointOfSale.Operations));

        if (pointOfSale is null)
        {
            Operation.Error(new SewingFactoryNotFoundException($"{nameof(PointOfSale)} with key {request.Id} was not found"));
        }

        return Operation.Result(_mapper.Map<PointOfSaleDetailsReadViewModel>(pointOfSale));
    }
}
