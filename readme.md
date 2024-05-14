# Order Aggregator - Interview task

This repository contains the implementation of an interview task for cbData.

It is an application implemented using ASP.NET Core 8 (and .NET 8 for that matter). The application simulates receiving orders in JSON format on a REST API endpoint, aggregating them based on their product ID, and simulating a send-off to an internal system by displaying them on the console.

## Overall Architecture

The application is implemented using three main components: 
* OrderService 
* OrderQueue
* OrderAggregator

### OrderService
This component processes and validates any incoming orders from the REST endpoint. The service then sends any orders to the OrderQueue.

### OrderQueue
This is a simulation of a message queue that accepts every incoming order as a single object/message.

### OrderAggregator
Implemented as a background service that runs as long as the server does. It obtains orders from the queue as fast as possible. The obtained orders are then aggregated based on product ID in an in-memory dictionary. Every 20 seconds, the contents of the dictionary are sent to a third-party system and the dictionary is emptied.

## Tests
Every component aims to be fully testable and covered by unit tests. Tests are implemented using XUnit without any additional libraries.