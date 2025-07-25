﻿using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Validators;

public sealed class CreateProcessViewModelValidator
    : AbstractValidator<CreateRequest<CreateProcessViewModel, Process, ReadProcessViewModel>>
{
    public CreateProcessViewModelValidator()
    {
        RuleFor(expression: x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.").MaximumLength(100);

        RuleFor(expression: x => x.Model.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}