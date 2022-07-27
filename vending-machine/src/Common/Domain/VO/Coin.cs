using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Domain.VO
{
    /// <summary>
    /// ValueObject that encapsulates a coin. Responsible for ensuring the integrity of your data.
    /// </summary>
    [Keyless]
    public class Coin : ValueObject
    {
        /// <summary>
        /// Coin name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Coin currency
        /// </summary>
        public string Currency { get; private set; }

        /// <summary>
        /// Monetary quantity
        /// </summary>
        public decimal Amount { get; private set; }

        public Coin(decimal amount, string currency = "EUR")
        {
            //Ensuring integrity of data
            if (!ValidCurrencies.Contains(currency))
            {
                throw new WrongCurrencyException($"The currency {currency} is not supported");
            }
            if (!ValidCoinAmounts.Contains(amount))
            {
                throw new WrongCoinAmountException($"The coin amount {amount} is not supported");
            }

            Amount = amount;
            Name = GetCoinName(Amount);
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Amount;
            yield return Currency;
            yield return Name;
        }


        //For simplicity, we store the currency and allowed coin values in private variables
        private readonly List<string> ValidCurrencies = new List<string>() { "EUR" };
        private readonly List<decimal> ValidCoinAmounts = new List<decimal>() { 0.10m, 0.20m, 0.50m, 1m, 2m };

        private string GetCoinName(decimal amount)
        {
            switch(amount)
            {
                case 0.10m:
                    return "10 cents";
                case 0.20m:
                    return "20 cents";
                case 0.50m:
                    return "50 cents";
                case 1m:
                    return "1 euro";
                case 2m:
                    return "2 euros";
                default:
                    return "";
            }
        }
    }
}
