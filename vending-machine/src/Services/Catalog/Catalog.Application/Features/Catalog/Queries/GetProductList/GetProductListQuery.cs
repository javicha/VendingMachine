using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Vending.Application.Features.Catalog.Queries.GetProductList
{
    /// <summary>
    /// CQRS pattern: Parameters of the query that obtains the list of products from the catalog of a vending machine
    /// </summary>
    public class GetProductListQuery : IRequest<List<ProductDTO>>
    {
        /// <summary>
        /// Vending machine unique serial number
        /// </summary>
        [Required]
        public string SerialNumber { get; set; }


        public GetProductListQuery(string serialNumber)
        {
            SerialNumber = serialNumber;
        }

        public GetProductListQuery()
        {
        }
    }
}
