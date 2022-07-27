using Domain.DTO;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Vending.Application.Features.Catalog.Commands.ReturnCoins
{
    /// <summary>
    /// CQRS pattern: Parameters of the command that returns the coins inserted in the machine
    /// </summary>
    public class ReturnCoinsCommand : IRequest<List<CoinDTO>>
    {
        /// <summary>
        /// Vending machine unique serial number
        /// </summary>
        [Required]
        public string SerialNumber { get; set; }

        public ReturnCoinsCommand(string serialNumber)
        {
            SerialNumber = serialNumber;
        }
    }
}
