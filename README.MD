# Event Sourcing

Event Sourcing is an architectural pattern that focuses on storing the state of an application as a sequence of events rather than a current state snapshot.

## Core Principles

1. **Events as the Source of Truth:**  
   In Event Sourcing, every change in the application's state is captured as an event. These events are stored immutably in an "Event Store." Each event describes a fact that occurred within the system, such as "Order Created," "Product Added to Cart," or "Payment Made."

2. **Rebuilding State:**  
   The application doesn't store the current state directly. Instead, the state is reconstructed by replaying all the events from the beginning. This approach provides a complete history of all changes, allowing you to revert to any previous state by replaying events up to that point.

3. **Eventual Consistency:**  
   In distributed architectures, the current state may not be immediately updated after each event. However, the system eventually reaches consistency once all components or services have processed the relevant events.

## Architecture

- **Application/**
  - **Commands/**: This folder contains the commands (`Command`) that represent the intentions to modify the state of the system. Each command is an action that needs to be executed, such as `CreateOrderCommand`, `UpdateUserProfileCommand`, etc.
  - **Queries/**: This folder includes the queries (`Query`) that are used to read data from the system. Queries are optimized for retrieving specific data, such as `GetOrderDetailsQuery`, `ListUserOrdersQuery`, etc.
  - **Handlers/**: Commands and Queries contains their command handlers. Command handlers are responsible for executing the application logic associated with a command, while query handlers handle the processing of read requests.
  - **Services/**: This folder contains the services (`Services`) that implement the business logic necessary to execute commands and respond to queries. Services can interact with repositories, other domain resources, or events to complete the required operations.


## Advantages

- **Complete History:**  
  Since all events are recorded, you have a full audit trail of everything that happened in the system. This is valuable for debugging, auditing, and future feature development.
- **Flexibility:**  
  You can easily add new business logic or views by reusing past events without altering the existing data model.
- **Performance and Scalability:**  
  Separating event writing from state reading allows you to optimize these operations independently.

## Challenges

- **Complexity:**  
  Managing events, rebuilding state, and handling event versioning can increase system complexity.
- **Error Handling:**  
  Since events are immutable, errors in events require careful management, often necessitating the creation of new compensating events.

## When to Use It

Event Sourcing is particularly useful in complex systems where maintaining a historical record of all events is critical, such as financial systems, e-commerce platforms, or applications with high auditing requirements.
