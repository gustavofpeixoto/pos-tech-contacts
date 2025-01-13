using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PosTech.Contacts.ApplicationCore;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Constants;
using PosTech.Contacts.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddApplicationCoreServices();

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

contacts.MapPost("/", async (IMediator mediator, 
    IValidator<CreateContactCommand> validator, CreateContactCommand createContactCommand) 
    => await CreateContactAsync(mediator, validator, createContactCommand));
static async Task<IResult> CreateContactAsync(IMediator mediator, IValidator<CreateContactCommand> validator,
    CreateContactCommand createContactCommand)
{
    var validationResult = await validator.ValidateAsync(createContactCommand);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
    
    await mediator.Send(createContactCommand);

    return TypedResults.Created();
}

app.Run();
