using FluentValidation;
using MediatR;
using appEx = Application.Exceptions;

namespace Vending.Application.Behaviours
{
    /// <summary>
    /// In this class we collect and run all the validations, in order to check the validation errors and throw the validation exception.
    /// The IPipelineBehavior interface allows us to intercept the request before or after executing the handler and add additional behavior (such as validations).
    /// More info: https://garywoodfine.com/how-to-use-mediatr-pipeline-behaviours/
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Collect all the validator that implements IValidators from the assembly using the FluentValidation reflection
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request); //Creates a context to perform validations

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    throw new appEx.ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
