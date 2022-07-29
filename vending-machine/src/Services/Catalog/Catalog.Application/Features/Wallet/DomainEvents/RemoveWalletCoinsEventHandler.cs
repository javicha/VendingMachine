using Application.EventDispatcher;
using Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;
using Vending.Domain.Events;
using Vending.Domain.ExtensionMethods;

namespace Vending.Application.Features.Wallet.DomainEvents
{
    /// <summary>
    /// CQRS pattern: RemoveWalletCoinsEvent domain event handler
    /// </summary>
    public class RemoveWalletCoinsEventHandler : INotificationHandler<DomainEventNotification<RemoveWalletCoinsEvent>>
    {
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly ILogger<RemoveWalletCoinsEventHandler> _logger;

        public RemoveWalletCoinsEventHandler(IVendingMachineRepository vendingMachineRepository, ILogger<RemoveWalletCoinsEventHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        /// <summary>
        /// Removes the coins returned to the client from the vending machine wallet and reset the client coins basket
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(DomainEventNotification<RemoveWalletCoinsEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            var vendingMachine = await _vendingMachineRepository.GetVendingMachineWithCoins(domainEvent.SerialNumber);
            if (vendingMachine == null)
            {
                throw new NotFoundException(nameof(VendingMachine), domainEvent.SerialNumber);
            }

            //Removing coins
            foreach (var c in domainEvent.Coins)
            {
                var coinToDelete = vendingMachine.Coins.Where(x => x.DateDeleted == null && x.Amount.Equals(c.Amount)).First();
                coinToDelete.SetDeleteAuditParams("userTest");
            }
            //Mark the rest of the coin as internal to reset the client coin basket
            var externalCoins = vendingMachine.Coins.Where(x => x.DateDeleted == null && x.External).ToList();
            externalCoins.ForEach(c => { c.SetAsInternal(); });
        }
    }
}
