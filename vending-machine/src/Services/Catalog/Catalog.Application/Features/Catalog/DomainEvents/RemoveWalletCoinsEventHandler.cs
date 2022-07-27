using Application.EventDispatcher;
using Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;
using Vending.Domain.Events;
using Vending.Domain.ExtensionMethods;

namespace Vending.Application.Features.Catalog.DomainEvents
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
        /// Return the difference between the inserted amount and the price using the smallest number of coins possible.
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
            foreach(var c in domainEvent.Coins)
            {
                var coinToDelete = vendingMachine.Coins.Where(x => x.DateDeleted == null && x.Amount.Equals(c.Amount)).First();
                coinToDelete.SetDeleteAuditParams("userTest");
            }
        }
    }
}
