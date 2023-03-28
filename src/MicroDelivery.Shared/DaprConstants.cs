namespace MicroDelivery.Shared
{
    public class DaprConstants
    {
        // Components
        public const string RabbitMqPubSubComponentName = "rabbitmqpubsub";
        public const string RedisStateComponentName = "redisstore";
        public const string MongoStateComponentName = "mongostore";
        public static string HttpBinding = "httpbinding";
        public static string SmtpBinding = "smtpbinding";
        public static string DiscountCronBinding = "discountcronbinding";
        public static string RedisConfigStore = "redisconfigstore";

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