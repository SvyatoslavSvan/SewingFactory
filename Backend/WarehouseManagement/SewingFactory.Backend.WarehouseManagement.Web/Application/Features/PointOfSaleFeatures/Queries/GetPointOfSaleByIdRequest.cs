using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries
{
    public record GetPointOfSaleByIdRequest(
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
        public override async Task<Operation<PointOfSaleDetailsReadViewModel, SewingFactoryNotFoundException>> Handle(
            GetByIdRequest<PointOfSale, PointOfSaleDetailsReadViewModel> request,
            CancellationToken cancellationToken)
        {
            var pointOfSale = await unitOfWork.GetRepository<PointOfSale>()
                .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id,
                    include: x => x.Include(pointOfSale => pointOfSale.StockItems).ThenInclude(stockItem => stockItem.GarmentModel));

            if (pointOfSale is null)
            {
                Operation.Error(new SewingFactoryNotFoundException($"{nameof(PointOfSale)} with key {request.Id} was not found"));
            }
            return Operation.Result(mapper.Map<PointOfSaleDetailsReadViewModel>(pointOfSale));
        }
    }
}
