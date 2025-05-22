using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Messaging.EventItemMessages.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Definitions.Mediator.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Definitions.Mediator
{
    public class LogPostTransactionBehavior : TransactionBehavior<IRequest<Operation<EventItemViewModel>>, Operation<EventItemViewModel>>
    {
        public LogPostTransactionBehavior(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}