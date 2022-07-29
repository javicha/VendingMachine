using FluentValidation;

namespace Vending.Application.Features.Wallet.Commands.ReturnCoins
{
    /// <summary>
    /// CQRS pattern: Pre processor behavior. Validator for ReturnCoinsCommand (we use fluent validation).
    /// Using constructor to provide the properties validations.
    /// </summary>
    public class ReturnCoinsCommandValidator : AbstractValidator<ReturnCoinsCommand>
    {
        public ReturnCoinsCommandValidator()
        {
            RuleFor(p => p.SerialNumber)
               .NotNull()
               .NotEmpty().WithMessage("{SerialNumber} must be a valid identifier.");
        }
    }
}
