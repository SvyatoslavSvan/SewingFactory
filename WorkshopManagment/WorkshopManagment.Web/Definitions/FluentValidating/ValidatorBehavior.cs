using FluentValidation;
using MediatR;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.FluentValidating;

/// <summary>
///     Base validator for requests
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidatorBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    ///     Pipeline handler. Perform any additional behavior and await the <paramref name="next" /> delegate as necessary
    /// </summary>
    /// <param name="request">Incoming request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
    /// <returns>Awaitable task returning the <typeparamref name="TResponse" /></returns>
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var types = validators.Select(selector: v => v.GetType().Name);
        var failures = validators
            .Select(selector: x => x.Validate(new ValidationContext<TRequest>(request)))
            .SelectMany(selector: x => x.Errors)
            .Where(predicate: x => x != null)
            .ToList();

        return failures.Any()
            ? throw new ValidationException(failures)
            : next();
    }
}