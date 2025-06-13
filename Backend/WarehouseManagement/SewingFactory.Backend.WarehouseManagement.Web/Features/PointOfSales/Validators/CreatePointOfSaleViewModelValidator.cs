using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Validators;

public sealed class CreatePointOfSaleViewModelValidator
    : AbstractValidator<CreateRequest<PointOfSaleCreateViewModel, PointOfSale, PointOfSaleReadViewModel>>
{
    public CreatePointOfSaleViewModelValidator() => RuleFor(expression: x => x.Model.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(100).WithMessage("Name must be at most 30 characters.");
}
