﻿using Application.EventDispatcher;
using Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;
using Vending.Domain.Events;

namespace Vending.Application.Features.Wallet.DomainEvents
{
    public class GetBackCoinsEventHandler : INotificationHandler<DomainEventNotification<GetBackCoinsEvent>>
    {
        private readonly IVendingMachineRepository _vendingMachineRepository;
        private readonly ILogger<GetBackCoinsEventHandler> _logger;

        public GetBackCoinsEventHandler(IVendingMachineRepository vendingMachineRepository, ILogger<GetBackCoinsEventHandler> logger)
        {
            _vendingMachineRepository = vendingMachineRepository ?? throw new ArgumentNullException(nameof(vendingMachineRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(DomainEventNotification<GetBackCoinsEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            var machineToUpdate = await _vendingMachineRepository.GetVendingMachineWithCoins(domainEvent.SerialNumber);
            if (machineToUpdate == null)
            {
                throw new NotFoundException(nameof(VendingMachine), domainEvent.SerialNumber);
            }

            machineToUpdate.RemoveExternalCoins();
            _logger.LogInformation("GetBackCoinsEventHandler - The coins inserted by the user have been successfully deleted");
        }
    }
}
