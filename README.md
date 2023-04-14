# MicroDelivery
A simple microservices solution using DAPR & .NET SDK.

![Overview](/docs/f.png)

### Customers Microservice
![Customers Microservice](/docs/c.png)
#### Url
*MicroDelivery.Customers.Api* (http://localhost:8000/swagger/index.html)
#### Description
It's a CRUD microservice to manage customers' data.
It persists its data in SQL Server using Entity Framework and caches them in MongoDB, using Dapr State block

### Products Microservice
![Products Microservice](/docs/p.png)
#### Url
*MicroDelivery.Products.Api* (http://localhost:8001/swagger/index.html)
#### Description
It's a CRUD microservice to manage products' data.
It persists its data in PostgreSQL using Entity Framework and caches them in Redis using Dapr State block

### Orders Microservice
![Orders Microservice](/docs/o.png)
#### Url
*MicroDelivery.Orders.Api* (http://localhost:8002/swagger/index.html)
#### Description
It's a microservice that receives the order requests, performs some logic, publishes a message (OrderSubmittedEvent) to notify other services, and receives a message (OrderShipped) to mark an order as shipped.
It persists its data in MongoDB, calls Discount microservice using service-to-service DAPR block, and send\received message in RabbitMQ using Dapr Pub\Sub block,

### Discount Microservice
![Discount Microservice](/docs/d.png)
#### Url
*MicroDelivery.Discount.Api* (http://localhost:8005/swagger/index.html)
#### Description
It's a microservice that, if enabled by configuration, calculates a random discount (very funny, isn't it?) that remains valid until the next recalculation.
The calculation is triggered by a Dapr CRON Binding, and the configuration is stored on the Redis configuration block. It will be invoked by the Orders microservice using Dapr service-to-service communication,

### Notifications Microservice
![Notifications Microservice](/docs/n.png)
#### Url
*MicroDelivery.Notifications.Api* (http://localhost:8004/swagger/index.html)
#### Description
It's a microservice that receives the message OrderSubmittedEvent and sends a confirmation email to customers, using Dapr SMTP binding.

### Shippings Microservice
![Shippings Microservice](/docs/s.png)
#### Url
*MicroDelivery.Shipping.Api* (http://localhost:8003/swagger/index.html)
#### Description
It's a microservice that receives the message OrderSubmittedEvent and performs an HTTP call to an external Webhook, using Dapr HTTP binding and reading the Bearer Token from Dapr Secret store.
