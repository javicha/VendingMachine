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
            if (productToSell.Price <= availableBalance)
            {
                coinsToReturn = CalculateDifference(availableBalance, productToSell.Price, vendingMachine.Coins.ToArray());
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
        private List<CoinDTO> CalculateDifference(decimal availableBalance, decimal productPrice, Coin[] totalCoins)
        {
            decimal difference = availableBalance - productPrice;
            return findMinCoins(totalCoins.Select(c => c.Amount).ToArray(), totalCoins.Count(), difference);
        }

        private List<CoinDTO> findMinCoins(decimal[] totalCoins, int size, decimal value)
        {
            List<CoinDTO> coinsToReturn = new List<CoinDTO>();
            int i, count = 0;
            //We perform the calculations with the bigger amounts first to use the minimum number of coins
            totalCoins = totalCoins.OrderByDescending(n => n).ToArray();

            for (i = 0; i < size; i++)
            {
                //take as much from coins[i]
                while (value >= totalCoins[i])
                {
                    //after taking the coin, reduce the value.
                    value -= totalCoins[i];
                    coinsToReturn.Add(new CoinDTO(totalCoins[i]));
                    count++;
                }
                if (value == 0)
                    break;

            }

            return coinsToReturn;
        }
    }
}