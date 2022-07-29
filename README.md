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
+  **Ordering.API**: Microservice for illustrative purposes (no Swagger UI). The only functionality it implements is subscribing to a Rabbit queue to consume the event "ReplenishStockEvent". **Each time you buy tea, this event is published by Vending.API** ('Tea' product is configured with this purpose) in the corresponding Rabbit queue, and this microservice consumes it and logs a message of the style *ReplenishStockConsumer - ReplenishStockEvent consumed - {event}*. We can see the message event in the logs, executing the command **docker compose logs ordering.api** from the command line.
+  **RabbitMQ**: We can access the Rabbit dashboard at the url http://localhost:7004/, using the default username and password (guest/guest). In this way we can also monitor the queues to see the published events.

![image](https://user-images.githubusercontent.com/3404380/181736008-ff86a87c-d3d5-45ee-89c5-4e38b63e339d.png)


Regarding the folder structure, we have a root folder with the code (src) and another with the tests (test), to separate the deployments from the test projects. Then inside the src folder, we start from the identification of the contexts of our system, and we will create these conceptual divisions within /src/{bounded_context} packages (for example, src/Vending, src/Ordering). If within each context, we identify several modules, we can in turn create subdivisions by modules (for example Vending/Product).

![image](https://user-images.githubusercontent.com/3404380/181736390-158a560b-1737-4450-afa4-d6c6e2b8f9b9.png)



##  Vending.API architecture
The service is designed following the principles of Clean Architecture DDD. Then, we structure the code into 4 layers:

![clean_architecture](https://user-images.githubusercontent.com/3404380/174666084-23ac18ef-88a5-49e5-abc1-eb64da12fedd.png)

**Vending.Domain** layer and **Vending.Application** layer will be the core layers. And we have **Vending.API** layer, which is the presentation layer, and **Vending.Infrastructure** layer (we also call Periphery layers). The main idea behind Clean Architecture approach is to separate the domain code from the application and infrastructure code, so that the core (business logic) of our software can evolve independently of the rest of the components. Regarding the DDD (Domain Drive Design) approach, it proposes modeling based on business reality according to its use cases. It is essential to organize the code to align with the business problems and use the same business terms (ubiquitous language).

We explain the layers in more detail:

+  **Vending.Domain**: It must contain the domain entities and encapsulate their business logic. And should **not have dependencies** on other application layers.
+  **Vending.Application**: This layer covers all business use cases, therefore it is responsible for aspects such as business use cases, business validations, business flows, etc. **Work only with abstractions**, delegating implementations to the infrastructure layer. Depends on Domain layer in order to use business entities and logic. We structure this layer into 3 main folders:
    -  Contracts: represent business requirements. It includes the interfaces and contracts for the application. This folder should cover application capabilities. This should include interfaces for abstracting use case implementations. We separate contracts into subfolders based on functionality.
    -  Features: represents the business use cases. It includes the application use cases and features, and the domain events. This folder will apply the CQRS design pattern for handling business use cases. It will contain a subfolder for each use case. Is the heart of this layer
    -  Behaviours: represents the business validations. It includes the business validations, logging, and other crosscutting concerns that apply when performing the use case implementations. 
+  **Vending.Infrastructure**: This layer will include the implementations of the abstractions defined in the Application layer. These are database operations, email sends operations, and all those related to external systems. It depends on the Application layer to use core layers.
+  **Vending.API**: This layer exposes API to external actors. Depends on the Application layer (to perform operations in controllers) and the Infrastructure layer.


##  Vending.API endpoints
These are the endpoints to interact with the vending machine:

![image](https://user-images.githubusercontent.com/3404380/181738761-b7f20095-deb8-46b1-b745-43f17926fa10.png)

**VERY IMPORTANT: You must use a SerialNumber parameter to interact with the vending machine. The serial number is "1234".** 



##  Design patterns and best practices
+  CQRS
+  Dependency Inversion
+  Dependency Injection
+  Logging
+  Validation
+  Exception handling
+  Repository
+  Unit of work
+  Testing
+  Implement with SOLID principles in mind


##  Third-party Nuget packages
+  **AutoMapper**: A convention-based object-object mapper. We use it to mapping operations between objects.
+  **AutoMapper.Extensions.Microsoft.DependencyInjection**: AutoMapper extensions for ASP.NET Core necessary to register AutoMapper in .NET Core dependency injection tool.
+  **FluentValidation**: A validation library for .NET that uses a fluent interface to construct strongly-typed validation rules. We ise it to perform validations when applying CQRS, before execute commands.
+  **FluentValidation.DependencyInjectionExtensions**: Dependency injection extensions for FluentValidation. We use it to being able to register FluentValidation in the NET Core dependency injection tool
+  **MassTransit**: MassTransit is a message-based distributed application framework for .NET. We use it in order to create queues in RabbitMQ, and thus, support asynchronous communication between microservices.
+  **MassTransit.RabbitMQ**: MassTransit RabbitMQ transport support. We use in order to connect to Rabbit MQ message broker.
+  **MediatR.Extensions.Microsoft.DependencyInjection**: MediatR extensions for ASP.NET Core. We use it in order to implement CQRS with Mediator pattern
+  **Moq.AutoMock**: An auto-mocking container that generates mocks using Moq
+  **Swashbuckle.AspNetCore**: Swagger tools for documenting APIs built on ASP.NET Core
+  **xunit**: xUnit.net is a developer testing framework, built to support Test Driven Development, with a design goal of extreme simplicity and alignment with framework features.


##  Assumptions
+  We use an in-memory database for simplicity
+  We only implement unit tests for the use case of adding product to inventory, to illustrate examples in the 3 layers of Clean Architecture
