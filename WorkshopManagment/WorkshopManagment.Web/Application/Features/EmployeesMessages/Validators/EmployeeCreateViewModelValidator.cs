using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Validators;

public class EmployeeCreateViewModelValidator
    : AbstractValidator<CreateRequest<EmployeeCreateViewModel, Employee, EmployeeReadViewModel>>
{
    public EmployeeCreateViewModelValidator()
    {
        RuleFor(expression: x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.").WithMessage("Name length cannot be less than 100");

        RuleFor(expression: x => x.Model.InternalId)
            .NotEmpty().WithMessage("Internal ID is required.");

        When(predicate: x => x.Model is ProcessEmployeeCreateViewModel, action: () =>
        {
            RuleFor(expression: x => ((ProcessEmployeeCreateViewModel)x.Model).Premium)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Premium must be non-negative");
        });

        When(predicate: x => x.Model is RateEmployeeCreateViewModel, action: () =>
        {
            RuleFor(expression: x => ((RateEmployeeCreateViewModel)x.Model).Rate)
                .NotNull().WithMessage("Rate is required.")
                .GreaterThan(0).WithMessage("Rate must be greater than 0.");
        });

        When(predicate: x => x.Model is TechnologistCreateViewModel, action: () =>
        {
            RuleFor(expression: x => ((TechnologistCreateViewModel)x.Model).SalaryPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("SalaryPercentage must be between 0 and 100.");
        });
    }
}