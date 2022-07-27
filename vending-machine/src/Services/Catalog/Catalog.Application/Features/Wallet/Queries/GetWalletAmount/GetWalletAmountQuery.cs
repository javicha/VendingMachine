using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Vending.Application.Features.Wallet.Queries.GetWalletAmount
{
    /// <summary>
    /// CQRS pattern: Parameters of the query that obtains total amount of money in the vending machine
    /// </summary>
    public class GetWalletAmountQuery : IRequest<decimal>
    {
        /// <summary>
        /// Vending machine unique serial number
        /// </summary>
        [Required]
        public string SerialNumber { get; set; }

        public GetWalletAmountQuery(string serialNumber)
        {
            SerialNumber = serialNumber ?? throw new ArgumentNullException(nameof(serialNumber));
        }

        public GetWalletAmountQuery()
        {
        }
    }
}
