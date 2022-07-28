using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;

var builder = WebApplication.CreateBuilder(args);
//WebApplicationBuilder returned by WebApplication.CreateBuilder(args) exposes Configuration and Environment properties
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();

// Add MassTransit and configure it with RabbitMQ connection
builder.Services.AddMassTransit(config => {

    // Declare the events consumers
    config.AddConsumer<ReplenishStockConsumer>();

    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(configuration["EventBusSettings:HostAddress"]);

        //Providing the queues to which each consumer subscribes
        cfg.ReceiveEndpoint(EventBusConstants.ReplenishStockQueue, c =>
        {
            c.ConfigureConsumer<ReplenishStockConsumer>(ctx);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
