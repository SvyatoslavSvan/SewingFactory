using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.IdentityServer.Web.Application.Messaging.EventItemMessages.ViewModels;
using SewingFactory.Backend.IdentityServer.Web.Definitions.Mediator.Base;

namespace SewingFactory.Backend.IdentityServer.Web.Definitions.Mediator
{
    public class EventItemPostTransactionBehavior(IUnitOfWork unitOfWork)
        : TransactionBehavior<IRequest<Operation<EventItemViewModel>>, Operation<EventItemViewModel>>(unitOfWork);
}