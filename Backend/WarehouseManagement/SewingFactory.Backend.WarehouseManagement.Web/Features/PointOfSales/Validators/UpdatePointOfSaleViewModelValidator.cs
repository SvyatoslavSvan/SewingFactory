using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Validators;

public class UpdatePointOfSaleViewModelValidator
    : AbstractValidator<UpdateRequest<PointOfSaleEditViewModel, PointOfSale>>
{
    public UpdatePointOfSaleViewModelValidator()
    {
        RuleFor(expression: x => x.Model.Id)
            .NotEmpty().WithMessage("PointOfSale Id is required.");

        RuleFor(expression: x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be at most 30 characters.");
    }
}
