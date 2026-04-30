using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQOrderSystem.Infrastructure.Messaging.Configurations
{
    public class RabbitMqSettings
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string Username { get; set; } = "admin";
        public string Password { get; set; } = "Admin@123";
        public string VirtualHost { get; set; } = "/";
    }
}
