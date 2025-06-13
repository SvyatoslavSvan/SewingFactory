using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Validators;

public class CreateGarmentCategoryValidator : AbstractValidator<CreateRequest<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel>>
{
    public CreateGarmentCategoryValidator()
    {
        RuleFor(expression: x => x.Model.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty")
            .MaximumLength(100)
            .WithMessage("Length cannot be more than 100 symbols");

        RuleFor(expression: x => x.Model.Description)
            .NotEmpty()
            .WithMessage("Name cannot be empty")
            .MaximumLength(1500)
            .WithMessage("Length cannot be more than 1500 symbols");

        RuleFor(expression: x => x.Model.GarmentCategoryId)
            .NotEmpty()
            .WithMessage("GarmentCategory is required");
    }
}