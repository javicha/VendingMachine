using Domain.DTO;

namespace Vending.Application.Features.Catalog.Commands.SellProduct
{
    /// <summary>
    /// DTO object with the necessary information regarding the sale of the product
    /// </summary>
    public class SellProductResponseDTO
    {
        /// <summary>
        /// The message shown by vending machine after the sell
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Coins returned by vending machine after the sell
        /// </summary>
        public List<CoinDTO> CoinsToReturn { get; set; }

        /// <summary>
        /// Flag to indicate if the minimum stock of the product was reached
        /// </summary>
        public bool MinStockReached { get; set; }

        public SellProductResponseDTO(string message, List<CoinDTO> coinsToReturn, bool minStockReached)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            CoinsToReturn = coinsToReturn ?? throw new ArgumentNullException(nameof(coinsToReturn));
            MinStockReached = minStockReached;
        }
    }
}
