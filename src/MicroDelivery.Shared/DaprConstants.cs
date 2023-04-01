namespace MicroDelivery.Shared
{
    public class DaprConstants
    {
        // Components
        public const string RabbitMqPubSubComponentName = "rabbitmqpubsub";
        public const string RedisStateComponentName = "redisstore";
        public const string MongoStateComponentName = "mongostore";
        public const string HttpBinding = "httpbinding";
        public const string SmtpBinding = "smtpbinding";
        public const string DiscountCronBinding = "discountcronbinding";
        public const string RedisConfigStore = "redisconfigstore";
        public const string LocalSecretStore = "localsecretstore";

        // Topics
        public const string OrderSubmittedEventTopic = "OrderSubmittedEventTopic";
        public const string OrderShippedEventTopic = "OrderShippedEventTopic";

        // Applications
        public const string AppIdCustomers = "microdelivery-customers-api";
        public const string AppIdProducts = "microdelivery-products-api";
        public const string AppIdOrders = "microdelivery-orders-api";
        public const string AppIdNotifications = "microdelivery-notifications-api";
        public const string AppIdShipping = "microdelivery-shippinh-api";
        public const string AppIdDiscount = "microdelivery-discount-api";


    }
}