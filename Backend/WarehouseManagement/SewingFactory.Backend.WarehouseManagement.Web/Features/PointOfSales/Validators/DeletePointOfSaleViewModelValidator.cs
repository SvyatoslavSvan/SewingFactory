using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Validators;

public class DeletePointOfSaleViewModelValidator
    : AbstractValidator<DeleteRequest<PointOfSaleDeleteViewModel, PointOfSale>>
{
    public DeletePointOfSaleViewModelValidator() => RuleFor(expression: x => x.Model.Id)
        .NotEmpty().WithMessage("PointOfSale Id is required.");
}
