using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Validators
{
    public class UpdatePointOfSaleViewModelValidator
        : AbstractValidator<UpdateRequest<PointOfSaleEditViewModel, PointOfSale>>
    {
        public UpdatePointOfSaleViewModelValidator()
        {
            RuleFor(x => x.Model.Id)
                .NotEmpty().WithMessage("PointOfSale Id is required.");

            RuleFor(x => x.Model.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be at most 30 characters.");
        }
    }
}
