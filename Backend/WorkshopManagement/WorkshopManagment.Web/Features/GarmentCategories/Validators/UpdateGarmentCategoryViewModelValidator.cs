using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Validators;

public class UpdateGarmentCategoryViewModelValidator
    : AbstractValidator<UpdateRequest<UpdateGarmentCategoryViewModel, GarmentCategory>>
{
    public UpdateGarmentCategoryViewModelValidator()
    {
        RuleFor(expression: x => x.Model.Id)
            .NotEmpty().WithMessage("Category Id is required.");

        RuleFor(expression: x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be at most 30 characters.");
    }
}