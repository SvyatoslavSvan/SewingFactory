using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Validators;

public class CreateGarmentModelValidator : AbstractValidator<CreateRequest<GarmentModelCreateViewModel, GarmentModel,GarmentModelReadViewModel>>
{
    public CreateGarmentModelValidator()
    {
        RuleFor(x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        RuleFor(x => x.Model.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");
        RuleFor(x => x.Model.CategoryId).NotEmpty();
    }
}
