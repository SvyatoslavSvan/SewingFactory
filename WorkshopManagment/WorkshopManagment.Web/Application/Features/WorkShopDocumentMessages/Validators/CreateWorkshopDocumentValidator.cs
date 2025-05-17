using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.Validators;

public class CreateWorkshopDocumentValidator : AbstractValidator<CreateRequest<CreateWorkshopDocumentViewModel, WorkshopDocument, DetailsReadWorkshopDocumentViewModel>>
{
    public CreateWorkshopDocumentValidator()
    {
        RuleFor(expression: x => x.Model.CountOfModelsInvolved).NotNull().WithMessage("Rate is required.")
            .GreaterThan(0).WithMessage("Rate must be greater than 0.");

        RuleFor(expression: x => x.Model.Name).NotEmpty().WithMessage("Name cannot be empty").MaximumLength(100);
        RuleFor(expression: x => x.Model.Date).NotEmpty();
        RuleFor(expression: x => x.Model.GarmentModelId).NotEmpty().WithMessage("GarmentModel is required");
        RuleFor(expression: x => x.Model.DepartmentId).NotEmpty().WithMessage("Department is required");
    }
}