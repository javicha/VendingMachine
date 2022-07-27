using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Vending.Application.Features.Catalog.Commands.AcceptCoin
{
    /// <summary>
    /// CQRS pattern: Parameters of the command to accept a coin in a vending machine. Expected result: total amount inserted
    /// </summary>
    public class AcceptCoinCommand : IRequest<decimal>
    {

        /// <summary>
        /// Vending machine unique serial number
        /// </summary>
        [Required]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Monetary quantity
        /// </summary>
        [Required]
        public decimal Amount { get; set; }


        public AcceptCoinCommand(string serialNumber, decimal amount)
        {
            SerialNumber = serialNumber;
            Amount = amount;
        }
    }
}
