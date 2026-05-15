using Microsoft.EntityFrameworkCore;
using RabbitMQOrderSystem.Infrastructure.Data;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));
    })
    .Build();

host.Run();