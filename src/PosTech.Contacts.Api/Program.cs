using AutoMapper;
using FluentValidation;
using MediatR;
using PosTech.Contacts.Api.Endpoints;
using PosTech.Contacts.ApplicationCore;
using PosTech.Contacts.ApplicationCore.Constants;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.Infrastructure;
using Prometheus;
using Prometheus.DotNetRuntime;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var prometheusBuilder = DotNetRuntimeStatsBuilder.Default();
prometheusBuilder = DotNetRuntimeStatsBuilder.Customize()
    .WithContentionStats(CaptureLevel.Informational)
    .WithGcStats(CaptureLevel.Verbose)
    .WithThreadPoolStats(CaptureLevel.Informational)
    .WithExceptionStats(CaptureLevel.Errors)
    .WithJitStats();

prometheusBuilder.RecycleCollectorsEvery(new TimeSpan(0, 20, 0));

prometheusBuilder.StartCollecting();

builder.Services.AddControllers();
builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddApplicationCoreServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseHttpMetrics();
app.UseMetricServer();

#region Contact Endpoits

RouteGroupBuilder contacts = app.MapGroup(RouteConst.Contacts);

contacts.MapPost("/", async (IMediator mediator, IMapper mapper,
    IValidator<CreateAndUpdateContactRequestDto> validator, CreateAndUpdateContactRequestDto contactDto)
    => await ContactEndpoint.CreateContactAsync(mediator, validator, mapper, contactDto))
    .WithOpenApi(op => new(op) { Description = "Endpoint para cadastro de novos contatos." })
    .Produces<ContactResponseDto>(StatusCodes.Status200OK);

contacts.MapPost("/search", async (IMediator mediator, IMapper mapper,
    IValidator<SearchContactRequestDto> validator, SearchContactRequestDto searchContactRequestDto)
    => await ContactEndpoint.SearchContactsAsync(mediator, mapper, validator, searchContactRequestDto))
    .WithOpenApi(op => new(op) { Description = @"Endpoint de busca avançada. 
Todos os parâmetros submetidos serão utilizados para filtrar os contatos do banco de dados." })
    .Produces<List<ContactResponseDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent);

contacts.MapGet("/{id:guid}", async (IMediator mediator, Guid id)
    => await ContactEndpoint.GetContactByIdAsync(mediator, id)).CacheOutput()
    .WithOpenApi(op => new(op) { Description = "Endpoint para busca de contatos por identificador único." })
    .Produces<ContactResponseDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent);

contacts.MapPatch("/{id:guid}", async (IMediator mediator, IMapper mapper,
    IValidator<CreateAndUpdateContactRequestDto> validator, CreateAndUpdateContactRequestDto contactDto, Guid id)
    => await ContactEndpoint.UpdateContactAsync(mediator, validator, mapper, contactDto, id))
    .WithOpenApi(op => new(op) { Description = "Entpoint para atualizar as informaões do contato." })
    .Produces<ContactResponseDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status422UnprocessableEntity);

contacts.MapDelete("/{id:guid}", async (IMediator mediator, Guid id)
    => await ContactEndpoint.DeleteContactAsync(mediator, id))
    .WithOpenApi(op => new(op) { Description = "Remove completamente as informações do contato da base de dados." })
    .Produces(StatusCodes.Status200OK);

#endregion

app.Run();
