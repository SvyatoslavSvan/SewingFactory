using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Validators;

public class UpdateGarmentModelValidator : AbstractValidator<UpdateRequest<GarmentModelEditViewModel, GarmentModel>>
{
    public UpdateGarmentModelValidator()
    {
        RuleFor(expression: x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(expression: x => x.Model.Price)
            .GreaterThan(0);

        RuleFor(expression: x => x.Model.Id).NotEmpty();
        RuleFor(expression: x => x.Model.CategoryId).NotEmpty();
    }
}
