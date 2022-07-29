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
