using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;

namespace Vending.Application.Features.Catalog.Commands.AcceptCoin
{
    public class AcceptCoinCommandHandler : IRequestHandler<AcceptCoinCommand, decimal>
    {
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AcceptCoinCommandHandler> _logger;

        public AcceptCoinCommandHandler(IVendingMachineRepository vendingMachineRepository, IMapper mapper, ILogger<AcceptCoinCommandHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<decimal> Handle(AcceptCoinCommand request, CancellationToken cancellationToken)
        {
            var machineToUpdate = await _vendingMachineRepository.GetVendingMachineWithCoins(request.SerialNumber);
            if (machineToUpdate == null)
            {
                throw new NotFoundException(nameof(VendingMachine), request.SerialNumber);
            }

            var totalInserted = machineToUpdate.InsertCoin(request.Amount);

            await _vendingMachineRepository.UpdateAsync(machineToUpdate, "userTest");

            _logger.LogInformation($"Coin {request.Amount.ToString()}€ is successfully inserted.");
            return totalInserted;
        }
    }
}
