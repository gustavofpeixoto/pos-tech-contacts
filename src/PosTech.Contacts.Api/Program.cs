using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Constants;
using PosTech.Contacts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

RouteGroupBuilder contacts = app.MapGroup(RouteConst.Contacts);

contacts.MapPost("/", async (CreateContactCommand createContactCommand, IMediator mediator)
    => await CreateContactAsync(createContactCommand, mediator));
static async Task<IResult> CreateContactAsync(CreateContactCommand createContactCommand, IMediator mediator)
{
    await mediator.Send(createContactCommand);
    
    return TypedResults.Ok();
}

app.Run();
