using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vending.Application.Features.Catalog.Queries.GetProductList;

namespace Vending.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VendingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VendingController> _logger;

        public VendingController(IMediator mediator, ILogger<VendingController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Obtains the catalog of available products of the vending machine
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <returns>The list of all available products</returns>
        [HttpGet]
        [Route("GetCatalog")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetCatalog([FromQuery] GetProductListQuery query)
        {
            _logger.LogInformation($"VendingController - GetCatalog");

            var products = await _mediator.Send(query); //Mediator is responsible for sending each query/command to its corresponding handler
            return Ok(products);
        }
    }
}
