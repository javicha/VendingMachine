using Domain.DTO;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Vending.Application.Features.Catalog.Commands.SellProduct
{
    /// <summary>
    /// CQRS pattern: Parameters of the command to sell a product
    /// </summary>
    public class SellProductCommand : IRequest<Tuple<string, List<CoinDTO>>>
    {
        /// <summary>
        /// Vending machine unique serial number
        /// </summary>
        [Required]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Product identifier
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        public SellProductCommand(string serialNumber, int productId)
        {
            SerialNumber = serialNumber;
            ProductId = productId;
        }
    }
}
