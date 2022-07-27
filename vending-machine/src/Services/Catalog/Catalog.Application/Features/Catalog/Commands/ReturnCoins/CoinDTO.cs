namespace Vending.Application.Features.Catalog.Commands.ReturnCoins
{
    /// <summary>
    /// DTO object with the Coin information customized for the presentation layer
    /// </summary>
    public class CoinDTO
    {   
        /// <summary>
        /// Monetary quantity
        /// </summary>
        public decimal Amount { get; private set; }

        public string GetName()
        {
            switch (Amount)
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
