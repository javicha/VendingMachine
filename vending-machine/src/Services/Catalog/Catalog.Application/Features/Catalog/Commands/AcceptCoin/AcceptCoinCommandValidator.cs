using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Vending.Domain.Entities;

namespace Vending.Application.Features.Catalog.Commands.AcceptCoin
{
    /// <summary>
    /// CQRS pattern: Pre processor behavior. Validator for AcceptCoinCommand (we use fluent validation).
    /// Using constructor to provide the properties validations.
    /// </summary>
    public class AcceptCoinCommandValidator : AbstractValidator<AcceptCoinCommand>
    {
        public AcceptCoinCommandValidator()
        {
            RuleFor(p => p.SerialNumber)
               .NotNull()
               .NotEmpty().WithMessage("{SerialNumber} must be a valid identifier.");

            RuleFor(p => p.Amount)
                .NotNull()
                .GreaterThan(0).WithMessage("{Amount} must be greater than zero.");
        }

        /// <summary>
        /// Advanced validation to ensure that it is a valid coin
        /// </summary>
        /// <returns>True if validation should continue, or False to immediately abort</returns>
        protected override bool PreValidate(ValidationContext<AcceptCoinCommand> context, ValidationResult result)
        {
            try
            {
                var validCoin = new Coin(context.InstanceToValidate.Amount);
            }
            catch(WrongCoinAmountException ex)
            {
                result.Errors.Add(new ValidationFailure("Amount", ex.Message));
                return false;
            }
            
            return true;
        }
    }
}
