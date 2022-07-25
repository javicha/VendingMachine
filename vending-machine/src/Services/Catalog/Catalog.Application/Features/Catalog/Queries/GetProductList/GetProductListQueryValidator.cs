using FluentValidation;

namespace Vending.Application.Features.Catalog.Queries.GetProductList
{
    /// <summary>
    /// CQRS pattern: Pre processor behavior. Validator for GetProductListQuery (we use fluent validation).
    /// Using constructor to provide the properties validations.
    /// </summary>
    public class GetProductListQueryValidator : AbstractValidator<GetProductListQuery>
    {
        public GetProductListQueryValidator()
        {
            RuleFor(p => p.VendigMachineId)
                .NotNull()
                .GreaterThan(0).WithMessage("{VendigMachineId} must be a valid identifier.");
        }
    }
}
