namespace RabbitMQOrderSystem.Infrastructure.Messaging.Configurations
{
    public class RedisSettings
    {
        public string Host { get; set; } = "redis";
        public int Port { get; set; } = 6379;
        public string Password { get; set; } = "Redis@123";
        public int DefaultExpiryMinutes { get; set; } = 30;
    }
}
