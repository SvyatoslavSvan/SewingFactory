using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Validators;

public class UpdateGarmentModelValidator : AbstractValidator<UpdateRequest<GarmentModelEditViewModel, GarmentModel>>
{
    public UpdateGarmentModelValidator()
    {
        RuleFor(x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        RuleFor(x => x.Model.Price)
            .GreaterThan(0);

        RuleFor(x => x.Model.Id).NotEmpty();
        RuleFor(x => x.Model.CategoryId).NotEmpty();
    }
}
