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
        /// Vending machine identifier
        /// </summary>
        [Required]
        public int VendigMachineId { get; set; }


        public GetProductListQuery(int vendigMachineId)
        {
            VendigMachineId = vendigMachineId;
        }

        public GetProductListQuery()
        {
        }
    }
}
