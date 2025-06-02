using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Validators;

public class DeletePointOfSaleViewModelValidator
    : AbstractValidator<DeleteRequest<PointOfSaleDeleteViewModel, PointOfSale>>
{
    public DeletePointOfSaleViewModelValidator() => RuleFor(expression: x => x.Model.Id)
        .NotEmpty().WithMessage("PointOfSale Id is required.");
}
