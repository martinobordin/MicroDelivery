# MicroDelivery
A simple microservices solution using __DAPR__ & __.NET SDK__.
Just run the solution with __Visual Studio__ using docker-compose and you'll have a full working solution with all the microservices and the DAPR sidecar containers.

# LinkedIn articles series
1. [An introduction to the runtime](https://www.linkedin.com/pulse/1-dapr-introduction-runtime-martino-bordin/)
2. [The application scenario](https://www.linkedin.com/pulse/2-dapr-application-scenario-martino-bordin/)
3. [Create a Dapr service](https://www.linkedin.com/pulse/3-dapr-create-service-martino-bordin/)
4. [State management](https://www.linkedin.com/pulse/4-dapr-state-management-martino-bordin/)
5. [Input Binding and Configuration](https://www.linkedin.com/pulse/5-dapr-input-binding-configuration-martino-bordin/)
6. [Service invocation & Pub/sub](https://www.linkedin.com/pulse/6-dapr-service-invocation-pubsub-martino-bordin/)
7. [Pub/sub & Output Binding](https://www.linkedin.com/pulse/7-dapr-pubsub-output-binding-martino-bordin/)
8. [Secret management](https://www.linkedin.com/pulse/8-dapr-secret-management-martino-bordin/)
9. [Resiliency](https://www.linkedin.com/pulse/9-dapr-resiliency-martino-bordin)
10. [Observability](https://www.linkedin.com/pulse/10-dapr-observability-martino-bordin)
11. [The end](https://www.linkedin.com/pulse/11-dapr-end-martino-bordin)

# Overview
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
Check the email accessing the SMTP dev server (http://localhost:5000)

### Shippings Microservice
![Shippings Microservice](/docs/s.png)
#### Url
*MicroDelivery.Shipping.Api* (http://localhost:8003/swagger/index.html)
#### Description
It's a microservice that receives the message OrderSubmittedEvent and performs an HTTP call to an external Webhook, using Dapr HTTP binding and reading the Bearer Token from Dapr Secret store.
Check the Webhook call accessing the EchoRest Bot (http://echorestbot.azurewebsites.net/history)
