using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.StockItems;
using SewingFactory.Backend.WarehouseManagement.Web.Endpoints.Base;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Endpoints
{
    public class PointOfSaleRouter
        : CommandRouter<PointOfSale,
            PointOfSaleReadViewModel,
            PointOfSaleCreateViewModel,
            PointOfSaleEditViewModel,
            PointOfSaleDeleteViewModel,
            PointOfSaleDetailsReadViewModel>
    {
        public override void ConfigureApplication(WebApplication app)
        {
            base.ConfigureApplication(app);
            app.MapPost(Prefix + "/addStockItem", AddStockItem).WithTags(_featureGroupName);
            app.MapPost(Prefix + "/sell", Sell).WithTags(_featureGroupName);
            app.MapPost(Prefix + "/writeOff", WriteOff).WithTags(_featureGroupName);
            app.MapPost(Prefix + "/receive", Receive).WithTags(_featureGroupName);
            app.MapPost(Prefix + "/internalTransfer", InternalTransfer).WithTags(_featureGroupName);
            app.MapGet(Prefix + "/leftOversReport", GetLeftOversReport).WithTags(_featureGroupName);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<Operation<StockItemReadViewModel, SewingFactoryNotFoundException, Exception>>
        AddStockItem(
            [FromBody] AddStockItemViewModel model,
            [FromServices] IMediator mediator,
            HttpContext context)
        => await mediator.Send(new AddStockItemRequest(model, context.User));

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<OperationEmpty<SewingFactoryNotFoundException, Exception>>
            Sell(
                [FromBody] CreateOperationViewModel model,
                [FromServices] IMediator mediator,
                HttpContext context)
            => await mediator.Send(new SellRequest(model, context.User));

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<OperationEmpty<SewingFactoryNotFoundException, Exception>>
            WriteOff(
                [FromBody] CreateOperationViewModel model,
                [FromServices] IMediator mediator,
                HttpContext context)
            => await mediator.Send(new WriteOffRequest(model, context.User));

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<OperationEmpty<SewingFactoryNotFoundException, Exception>>
            Receive(
                [FromBody] CreateOperationViewModel model,
                [FromServices] IMediator mediator,
                HttpContext context)
            => await mediator.Send(new ReceiveRequest(model, context.User));

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<OperationEmpty<SewingFactoryNotFoundException, Exception>>
            InternalTransfer(
                [FromBody] CreateInternalTransferViewModel model,
                [FromServices] IMediator mediator,
                HttpContext context)
            => await mediator.Send(new InternalTransferRequest(model, context.User));

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        public async Task<IResult> GetLeftOversReport(
            [FromServices] IMediator mediator, [FromQuery] Guid pointOfSaleId,
            HttpContext context)
            => await mediator.Send(new GetLeftOversReportRequest(context.User, pointOfSaleId),
                context.RequestAborted);
    }
}
