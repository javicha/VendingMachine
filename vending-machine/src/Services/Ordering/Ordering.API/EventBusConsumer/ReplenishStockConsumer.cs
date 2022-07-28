using EventBus.Messages.Events;
using MassTransit;
using System.Text.Json;

namespace Ordering.API.EventBusConsumer
{
    /// <summary>
    ///  Class that is subscribed to the ReplenishStockEvent event
    ///  Only for illustrative purposes. Does not perform any action. It only receives the event to which it is subscribed, and logs it
    /// </summary>
    public class ReplenishStockConsumer : IConsumer<ReplenishStockEvent>
    {
        private readonly ILogger<ReplenishStockConsumer> _logger;

        public ReplenishStockConsumer(ILogger<ReplenishStockConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public Task Consume(ConsumeContext<ReplenishStockEvent> context)
        {
            _logger.LogInformation($"ReplenishStockConsumer - ReplenishStockEvent consumed - {JsonSerializer.Serialize(context.Message)}");

            /*
             * Note
             * Some examples of functionality at this point: 
             *  - We could implement CQRS, creating a handler for the event, and execute it, in order to, for example, create an order in our database
             *  - Or we could send an email to a list of users with a certain management role.
             *  - Display a notification in our front in real time
             */

            return Task.CompletedTask;
        }
    }
}
