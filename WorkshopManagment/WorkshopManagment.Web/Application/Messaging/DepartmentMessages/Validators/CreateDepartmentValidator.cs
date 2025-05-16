using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Validators;

public class CreateDepartmentValidator : AbstractValidator<CreateRequest<CreateDepartmentViewModel, Department, ReadDepartmentViewModel>>
{
    public CreateDepartmentValidator() => RuleFor(expression: x => x.Model.Name).NotEmpty().WithMessage("Name cannot be Empty").MaximumLength(100);
}