using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Vending.Application.Features.Catalog.Commands.AcceptCoin;
using Vending.Application.Features.Catalog.Commands.ReturnCoins;
using Vending.Application.Features.Catalog.Commands.SellProduct;
using Vending.Application.Features.Catalog.Queries.GetProductList;
using Vending.Application.Features.Wallet.Queries.GetWalletAmount;

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
        /// Gets the total monetary value stored in the vending machine
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <returns>The total monetary value</returns>
        [HttpGet]
        [Route("GetTotalAmount")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<decimal>> GetTotalAmount([FromQuery] GetWalletAmountQuery query)
        {
            _logger.LogInformation($"VendingController - GetCatalog - {JsonSerializer.Serialize(query)}");

            var totalAmount = await _mediator.Send(query); //Mediator is responsible for sending each query/command to its corresponding handler
            return Ok(totalAmount.ToString("0.00"));
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


        /// <summary>
        /// Cancel the purchase and get back all the coins entered in the vending machine
        /// </summary>
        /// <param name="command">The vending machine serial number</param>
        /// <returns>All the coins inserted so far</returns>
        [HttpPost]
        [Route("CancelPurchase")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> ReturnCoins([FromBody] ReturnCoinsCommand command)
        {
            _logger.LogInformation($"VendingController - ReturnCoins - {JsonSerializer.Serialize(command)}");

            var result = await _mediator.Send(command);
            return Ok(result.Select(r => r.GetName()).ToList());
        }


        /// <summary>
        /// Endpoint that allows the customer to buy a product
        /// </summary>
        /// <param name="command">The vending machine serial number and product identifier</param>
        /// <returns>If the product price is less than the deposited amount, the Vending machine shows a “Thank you” message 
        /// and return the difference between the inserted amount and the price using the smallest number of coins possible.
        /// If the product price is higher than the amount inserted, Vending machine shows a message “Insufficient amount”
        /// </returns>
        [HttpPost]
        [Route("SellProduct")]
        [ProducesResponseType(typeof(Tuple<string, List<string>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> SellProduct([FromBody] SellProductCommand command)
        {
            _logger.LogInformation($"VendingController - SellProduct - {JsonSerializer.Serialize(command)}");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
