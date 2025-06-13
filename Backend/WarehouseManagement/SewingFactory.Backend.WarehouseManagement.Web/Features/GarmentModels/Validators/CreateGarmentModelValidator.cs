using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Validators;

public class CreateGarmentModelValidator : AbstractValidator<CreateRequest<GarmentModelCreateViewModel, GarmentModel, GarmentModelReadViewModel>>
{
    public CreateGarmentModelValidator()
    {
        RuleFor(expression: x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(expression: x => x.Model.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        RuleFor(expression: x => x.Model.CategoryId).NotEmpty();
    }
}
