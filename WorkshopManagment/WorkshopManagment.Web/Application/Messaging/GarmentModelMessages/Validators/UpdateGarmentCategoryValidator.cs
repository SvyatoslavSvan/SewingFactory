using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.Validators;

public class UpdateGarmentCategoryValidator : AbstractValidator<UpdateRequest<UpdateGarmentModelViewModel, GarmentModel>>
{
    public UpdateGarmentCategoryValidator()
    {
        RuleFor(expression: x => x.Model.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(expression: x => x.Model.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty")
            .MaximumLength(100)
            .WithMessage("Length cannot be more than 50 symbols");

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