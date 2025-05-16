using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Validators;

public class EmployeeUpdateViewModelValidator : AbstractValidator<UpdateRequest<EmployeeUpdateViewModel, Employee>>
{
    public EmployeeUpdateViewModelValidator()
    {
        RuleFor(expression: x => x.Model.Id)
            .NotEmpty();

        RuleFor(expression: x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.").MaximumLength(100).WithMessage("Name length cannot be less than 100");

        RuleFor(expression: x => x.Model.InternalId)
            .NotEmpty().WithMessage("Internal ID is required.");

        When(predicate: x => x.Model is ProcessEmployeeUpdateViewModel, action: () =>
        {
            RuleFor(expression: x => ((ProcessEmployeeUpdateViewModel)x.Model).Premium)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Premium must be non-negative");
        });

        When(predicate: x => x.Model is RateEmployeeUpdateViewModel, action: () =>
        {
            RuleFor(expression: x => ((RateEmployeeUpdateViewModel)x.Model).Rate)
                .NotNull().WithMessage("Rate is required.")
                .GreaterThan(0).WithMessage("Rate must be greater than 0.");
        });

        When(predicate: x => x.Model is TechnologistUpdateViewModel, action: () =>
        {
            RuleFor(expression: x => ((TechnologistUpdateViewModel)x.Model).SalaryPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("SalaryPercentage must be between 0 and 100.");
        });
    }
}