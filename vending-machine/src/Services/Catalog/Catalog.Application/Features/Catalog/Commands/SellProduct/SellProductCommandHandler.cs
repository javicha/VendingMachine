using Application.Exceptions;
using AutoMapper;
using Domain.DTO;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;

namespace Vending.Application.Features.Catalog.Commands.SellProduct
{
    /// <summary>
    /// CQRS pattern: SellProductCommand command handler
    /// </summary>
    public class SellProductCommandHandler : IRequestHandler<SellProductCommand, SellProductResponseDTO>
    {
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SellProductCommandHandler> _logger;

        public SellProductCommandHandler(IVendingMachineRepository vendingMachineRepository, IProductRepository productRepository, 
            IMapper mapper, ILogger<SellProductCommandHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<SellProductResponseDTO> Handle(SellProductCommand request, CancellationToken cancellationToken)
        {
            //Get vending machine in order to check product and price
            var vendingMachine = await _vendingMachineRepository.GetVendingMachineWithProductsAndCoins(request.SerialNumber);
            if (vendingMachine == null)
            {
                throw new NotFoundException(nameof(VendingMachine), request.SerialNumber);
            }

            //Get product to sell
            var productToSell = vendingMachine.Products.Where(p => p.Id == request.ProductId).FirstOrDefault();
            if (productToSell == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            //Check price and coins
            var coinsToReturn = new List<CoinDTO>();
            var availableBalance = vendingMachine.Coins.Where(c => c.External).Sum(c => c.Amount);
            if(productToSell.Price <= availableBalance)
            {
                coinsToReturn = CalculateDifference(availableBalance, productToSell.Price);
                bool minStock = productToSell.SellProduct(coinsToReturn, vendingMachine.SerialNumber);

                await _productRepository.UpdateAsync(productToSell, "userTest");
                return new SellProductResponseDTO($"Thank you! Enjoy your {productToSell.Name}", coinsToReturn, minStock);
            }
            else
            {
                return new SellProductResponseDTO("Insufficient amount", coinsToReturn, false);
            }
        }

        /// <summary>
        /// Return the difference between the inserted amount and the price using the smallest number of coins possible.
        /// </summary>
        /// <returns></returns>
        private List<CoinDTO> CalculateDifference(decimal availableBalance, decimal productPrice)
        {
            //TODO
            return new List<CoinDTO>() { new CoinDTO() { Amount = 1M }, new CoinDTO() { Amount = 0.20M } };
        }
    }
}
