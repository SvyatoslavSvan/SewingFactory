using Calabonga.OperationResults;
using MediatR;
using SewingFactory.Backend.IdentityServer.Web.Application.Features.Profile.ViewModels;
using SewingFactory.Backend.IdentityServer.Web.Application.Services;

namespace SewingFactory.Backend.IdentityServer.Web.Application.Features.Profile.Queries;

/// <summary>
///     Register new account
/// </summary>
public static class RegisterAccount
{
    public sealed class Request(RegisterViewModel model) : IRequest<Operation<UserProfileViewModel, string>>
    {
        public RegisterViewModel Model { get; } = model;
    }

    public class Handler(IAccountService accountService)
        : IRequestHandler<Request, Operation<UserProfileViewModel, string>>
    {
        public Task<Operation<UserProfileViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
            => accountService.RegisterAsync(request.Model, cancellationToken);
    }
}
