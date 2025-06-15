using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Validators;

public sealed class UpdateWorkshopDocumentValidator : AbstractValidator<UpdateRequest<UpdateWorkshopDocumentViewModel, WorkshopDocument>>
{
    public UpdateWorkshopDocumentValidator()
    {
        RuleFor(expression: x => x.Model.Id).NotEmpty();
        RuleFor(expression: x => x.Model.CountOfModelsInvolved).NotNull().WithMessage("Rate is required.")
            .GreaterThan(0).WithMessage("Rate must be greater than 0.");

        RuleFor(expression: x => x.Model.Name).NotEmpty().WithMessage("Name cannot be empty").MaximumLength(100);
        RuleFor(expression: x => x.Model.Date).NotEmpty();
    }
}