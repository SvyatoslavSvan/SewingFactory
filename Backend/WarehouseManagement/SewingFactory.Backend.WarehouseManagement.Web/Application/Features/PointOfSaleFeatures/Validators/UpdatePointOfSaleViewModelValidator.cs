using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Validators;

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
