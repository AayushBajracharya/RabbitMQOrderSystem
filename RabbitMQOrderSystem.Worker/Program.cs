using RabbitMQOrderSystem.Infrastructure.Messaging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddRabbitMq(hostContext.Configuration);
    })
    .Build();

host.Run();