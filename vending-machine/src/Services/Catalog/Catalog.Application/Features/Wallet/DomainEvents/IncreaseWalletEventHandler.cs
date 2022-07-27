using Application.EventDispatcher;
using Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;
using Vending.Domain.Events;

namespace Vending.Application.Features.Wallet.DomainEvents
{
    /// <summary>
    /// CQRS pattern: IncreaseWalletEvent domain event handler
    /// </summary>
    public class IncreaseWalletEventHandler : INotificationHandler<DomainEventNotification<IncreaseWalletEvent>>
    {
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly ILogger<IncreaseWalletEventHandler> _logger;

        public IncreaseWalletEventHandler(IVendingMachineRepository vendingMachineRepository, ILogger<IncreaseWalletEventHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Update the wallet of the vending machine by adding a coin with the indicated amount
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task Handle(DomainEventNotification<IncreaseWalletEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            var machineToUpdate = await _vendingMachineRepository.GetVendingMachineWithCoins(domainEvent.SerialNumber);
            if (machineToUpdate == null)
            {
                throw new NotFoundException(nameof(VendingMachine), domainEvent.SerialNumber);
            }

            machineToUpdate.IncreaseWallet(domainEvent.Amount);
            _logger.LogInformation($"IncreaseWalletEventHandler - Wallet increased in {domainEvent.Amount.ToString("0.00")}€");
        }
    }
}
