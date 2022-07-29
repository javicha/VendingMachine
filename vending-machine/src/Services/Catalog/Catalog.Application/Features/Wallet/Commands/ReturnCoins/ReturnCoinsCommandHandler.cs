using Application.Exceptions;
using AutoMapper;
using Domain.DTO;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;

namespace Vending.Application.Features.Wallet.Commands.ReturnCoins
{
    /// <summary>
    /// CQRS pattern: ReturnCoinsCommand command handler
    /// </summary>
    public class ReturnCoinsCommandHandler : IRequestHandler<ReturnCoinsCommand, List<CoinDTO>>
    {
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReturnCoinsCommandHandler> _logger;

        public ReturnCoinsCommandHandler(IVendingMachineRepository vendingMachineRepository, IMapper mapper, ILogger<ReturnCoinsCommandHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handler that returns the coins inserted in the machine
        /// </summary>
        /// <param name="request">Command parameters</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List representing the coins inserted in the machine</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<List<CoinDTO>> Handle(ReturnCoinsCommand request, CancellationToken cancellationToken)
        {
            var machineToUpdate = await _vendingMachineRepository.GetVendingMachineWithCoins(request.SerialNumber);
            if (machineToUpdate == null)
            {
                throw new NotFoundException(nameof(VendingMachine), request.SerialNumber);
            }

            var coins = machineToUpdate.ReturnCoins().OrderBy(c => c.Amount).ToList();
            await _vendingMachineRepository.UpdateAsync(machineToUpdate, "userTest");

            _logger.LogInformation($"Get {coins.Count} coins to return. A total of {coins.Sum(c => c.Amount)}€");
            return coins != null ? _mapper.Map<List<CoinDTO>>(coins) : new List<CoinDTO>();
        }
    }
}
