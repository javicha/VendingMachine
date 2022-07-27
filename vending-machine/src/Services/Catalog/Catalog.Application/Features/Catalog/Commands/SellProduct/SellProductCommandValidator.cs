using FluentValidation;

namespace Vending.Application.Features.Catalog.Commands.SellProduct
{
    /// <summary>
    /// CQRS pattern: Pre processor behavior. Validator for SellProductCommand (we use fluent validation).
    /// Using constructor to provide the properties validations.
    /// </summary>
    public class SellProductCommandValidator : AbstractValidator<SellProductCommand>
    {
        public SellProductCommandValidator()
        {
            RuleFor(p => p.SerialNumber)
               .NotNull()
               .NotEmpty().WithMessage("{SerialNumber} must be a valid identifier.");

            RuleFor(p => p.ProductId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0).WithMessage("{ProductId} must be a valid identifier.");
        }
    }
}
