using PosTech.Contacts.ApplicationCore.Repositories.Query;
using PosTech.Contacts.Infrastructure;
using PosTech.Contacts.Infrastructure.Settings;
using PosTech.Contacts.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFileByName("sharedsettings");
builder.Services.AddInfrastructureServices(builder.Configuration);

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();

builder.Services.AddHostedService(sp =>
{
    var contactRepository = sp.GetRequiredService<IContactRepository>();
    return new ContactCreatedMessageConsumer(contactRepository, rabbitMqSettings);
});

builder.Services.AddHostedService(sp =>
{
    var contactRepository = sp.GetRequiredService<IContactRepository>();
    return new ContactUpdatedMessageConsumer(contactRepository, rabbitMqSettings);
});


builder.Services.AddHostedService(sp =>
{
    var contactRepository = sp.GetRequiredService<IContactRepository>();
    return new ContactDeletedMessageConsumer(contactRepository, rabbitMqSettings);
});


var host = builder.Build();

host.Run();
