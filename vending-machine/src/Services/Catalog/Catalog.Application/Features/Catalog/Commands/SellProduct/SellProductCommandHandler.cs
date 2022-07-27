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
    public class SellProductCommandHandler : IRequestHandler<SellProductCommand, Tuple<string, List<CoinDTO>>>
    {
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SellProductCommandHandler> _logger;

        public SellProductCommandHandler(IVendingMachineRepository vendingMachineRepository, IMapper mapper, ILogger<SellProductCommandHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<Tuple<string, List<CoinDTO>>> Handle(SellProductCommand request, CancellationToken cancellationToken)
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
            var availablBalance = vendingMachine.Coins.Sum(c => c.Amount);
            if(productToSell.Price <= availablBalance)
            {
                //TODO buy
                return new Tuple<string, List<CoinDTO>>($"Thank you! Enjoy your {productToSell.Name}", new List<CoinDTO>());
            }
            else
            {
                return new Tuple<string, List<CoinDTO>>("Insufficient amount", new List<CoinDTO>());
            }
        }
    }
}
