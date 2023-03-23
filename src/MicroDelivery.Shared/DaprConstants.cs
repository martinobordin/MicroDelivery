namespace MicroDelivery.Shared
{
    public class DaprConstants
    {
        // Components
        public const string RedisStateComponentName = "redisstore";
        public const string MongoStateComponentName = "mongostore";
        public const string PubSubComponentName = "pubsub";
        public static string HttpBinding = "httpbinding";
        public static string SmtpBinding = "smtpbinding";

        // Topics
        public const string OrderSubmittedEventTopic = "OrderSubmittedEventTopic";

        // Applications
        public const string AppIdCustomers = "microdelivery-customers-api";
        public const string AppIdProducts = "microdelivery-products-api";
        public const string AppIdOrders = "microdelivery-orders-api";
        public const string AppIdNotifications = "microdelivery-notifications-api";
    }
}