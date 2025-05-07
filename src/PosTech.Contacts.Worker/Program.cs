using PosTech.Contacts.Infrastructure;
using PosTech.Contacts.Infrastructure.Settings;
using PosTech.Contacts.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFileByName("sharedsettings");
builder.Services.AddInfrastructureServices(builder.Configuration);

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();

builder.Services.AddHostedService(x => 
{
    return new ContactCreatedMessageConsumer(rabbitMqSettings);
});

var host = builder.Build();
host.Run();
