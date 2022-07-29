# VendingMachine
Example of a model of virtual vending machine, in an event driven way, adhering to the CQRS principles. It provides examples of Domain Events and asynchronous communication between microservices through external events and Rabbit MQ queues.

##  Requirements
+  .NET 6
+  Docker installed (the application runs in containers)

##  How to run the application
+  Clone the repository
+ You have two options:
  + 1 - Open the solution in Visual Studio to run and debug it.
  + 2 - Run it with Docker:
    +  Open a terminal and navigate to *VendingMachine/vending-machine/* path
    + Launch the application by running the following Docker command: **docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d** (this command build and run all the required images). *NOTE: RabbitMQ needs more time to get up. It is normal that in the first moments some connection error traces appear in the microservices that connect to RabbitMQ. It is not a problem. Connection retries are implemented.*
    +  Open a browser and navigate to http://localhost:7000/swagger/index.html to interact with the vending machine API using Swagger UI

## How to stop the application
If you are running the application using Docker, you can stop it by the command line. Located on *VendingMachine/vending-machine/* path, run the following Docker command to stop all the containers: **docker-compose -f docker-compose.yml -f docker-compose.override.yml down**

##  How to run test
For illustrative purposes, only one test for layer has been implemented. Located on *VendingMachine/vending-machine/tests/Vending.Test* path, execute the **dotnet test** command.


##  Not implemented requirements
+  A frontend has not been implemented to consume the API. All interactions must be done through Swagger


##  Solution architecture
A view of the global architecture of the application is shown:

![174652645-ce286a7f-635c-4ac8-a5da-178280a87ac1](https://user-images.githubusercontent.com/3404380/181731175-b586c870-9d07-44db-9503-6cf9728524ad.png)


The solution contains 2 microservices, with asynchronous communication mechanism through RabbitMQ. The main microservice is Vending.API, which provides all functionalities to interact with the vending machine. There is an Ordering.API microservice too, but only with illustrative purposes, to show the async communication between both. Ordering.API only receives the event and logs it.   

The domain events are dispatched using MediatR package and, for simplicity, processed before committing data into the DB. The external events are managed through asynchronous communication between microservices, using the MassTransit library (it has native support for Rabbit). A common class library contains the external events definitions. In this way, each microservice that needs external events will add a reference to this library. Brief description of the services:

+  **Vending.API**: Main microservice. It implements the vending machine functionalities. Accessible at the url http://localhost:7000/swagger/index.html. It includes:
    +  ASP.NET Core Web API application
    +  REST API principles
    +  Implementing DDD, CQRS, and Clean Architecture using Best Practices applying SOLID principles. Developing CQRS implementation on commands and queries using MediatR, FluentValidation and AutoMapper packages.
    +  InMemory database connection
    +  Using Entity Framework Core ORM and database initialization with test data entities when application startup
    +  Publishing RabbitMQ *ReplenishStockEvent* event queue using MassTransit-RabbitMQ Configuration
+  **Ordering.API**: Microservice for illustrative purposes (no swagger). The only functionality it implements is subscribing to a Rabbit queue to consume the event "ReplenishStockEvent". **Each time you buy tea, this event is published by Vending.API** ('Tea' product is configured with this purpose) in the corresponding Rabbit queue, and this microservice consumes it and logs a message of the style *ReplenishStockConsumer - ReplenishStockEvent consumed - {event}*. We can see the message event in the logs, executing the command **docker logs ordering.api** from the command line.
+  **RabbitMQ**: We can access the Rabbit dashboard at the url http://localhost:7004/, using the default username and password (guest/guest). In this way we can also monitor the queues to see the published events.


