using AutoMapper;
using FluentValidation;
using MediatR;
using PosTech.Contacts.Api.Endpoints;
using PosTech.Contacts.ApplicationCore;
using PosTech.Contacts.ApplicationCore.Constants;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using PosTech.Contacts.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddApplicationCoreServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

#region Contact Endpoits

RouteGroupBuilder contacts = app.MapGroup(RouteConst.Contacts);

contacts.MapPost("/", async (IMediator mediator, IMapper mapper,
    IValidator<CreateAndUpdateContactRequestDto> validator, CreateAndUpdateContactRequestDto contactDto)
    => await ContactEndpoint.CreateContactAsync(mediator, validator, mapper, contactDto));

contacts.MapPost("/search", async (IMediator mediator, IMapper mapper,
    IValidator<SearchContactRequestDto> validator, SearchContactRequestDto searchContactRequestDto)
    => await ContactEndpoint.SearchContactsAsync(mediator, mapper, validator, searchContactRequestDto)).CacheOutput();

contacts.MapGet("/{id:guid}", async (IMediator mediator, Guid id)
    => await ContactEndpoint.GetContactByIdAsync(mediator, id)).CacheOutput();

contacts.MapPatch("/{id:guid}", async (IMediator mediator, IMapper mapper,
    IValidator<CreateAndUpdateContactRequestDto> validator, CreateAndUpdateContactRequestDto contactDto, Guid id)
    => await ContactEndpoint.UpdateContactAsync(mediator, validator, mapper, contactDto, id));

contacts.MapDelete("/{id:guid}", async (IMediator mediator, Guid id)
    => await ContactEndpoint.DeleteContactAsync(mediator, id));

#endregion

app.Run();
