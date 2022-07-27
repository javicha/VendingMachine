using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;

namespace Vending.Application.Features.Wallet.Queries.GetWalletAmount
{
    /// <summary>
    /// CQRS pattern: GetWalletAmountQuery query handler
    /// </summary>
    public class GetWalletAmountQueryHandler : IRequestHandler<GetWalletAmountQuery, decimal>
    {
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWalletAmountQueryHandler> _logger;

        public GetWalletAmountQueryHandler(IVendingMachineRepository vendingMachineRepository, IMapper mapper, ILogger<GetWalletAmountQueryHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handler whose responsibility is to obtain the total amount of money in the vending machine
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<decimal> Handle(GetWalletAmountQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start handler - GetWalletAmountQueryHandler");

            var vendingMachine = await _vendingMachineRepository.GetVendingMachineWithCoins(request.SerialNumber);
            return vendingMachine != null && vendingMachine.Coins != null ? vendingMachine.Coins.Sum(c => c.Amount) : 0M;
        }
    }
}
