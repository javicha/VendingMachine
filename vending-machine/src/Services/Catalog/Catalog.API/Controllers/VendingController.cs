using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Vending.Application.Features.Catalog.Commands.AcceptCoin;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetCatalog([FromQuery] GetProductListQuery query)
        {
            _logger.LogInformation($"VendingController - GetCatalog - {JsonSerializer.Serialize(query)}");

            var products = await _mediator.Send(query); //Mediator is responsible for sending each query/command to its corresponding handler
            return Ok(products);
        }


        /// <summary>
        /// Insert a coin in the vending machine
        /// </summary>
        /// <param name="command">The vending machine serial number and the amount to be inserted</param>
        /// <returns>The total amount entered so far</returns>
        [HttpPost]
        [Route("AcceptCoin")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> AcceptCoin([FromBody] AcceptCoinCommand command)
        {
            _logger.LogInformation($"VendingController - AcceptCoin - {JsonSerializer.Serialize(command)}");

            var result = await _mediator.Send(command);
            return Ok(result.ToString("0.00"));
        }
    }
}
