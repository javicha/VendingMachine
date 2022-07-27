using FluentValidation;

namespace Vending.Application.Features.Wallet.Queries.GetWalletAmount
{
    /// <summary>
    /// CQRS pattern: Pre processor behavior. Validator for GetWalletAmountQuery (we use fluent validation).
    /// Using constructor to provide the properties validations.
    /// </summary>
    public class GetWalletAmountQueryValidator : AbstractValidator<GetWalletAmountQuery>
    {
        public GetWalletAmountQueryValidator()
        {
            RuleFor(p => p.SerialNumber)
                .NotNull()
                .NotEmpty().WithMessage("{SerialNumber} must be a valid identifier.");
        }
    }
}
